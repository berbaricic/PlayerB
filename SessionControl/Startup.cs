using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using RabbitMqEventBus;
using Autofac;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Hangfire;
using Autofac.Extensions.DependencyInjection;
using System;
using HangfireWorker.StorageConnection;
using HangfireWorker.StorageWork;
using HangfireWorker;
using Hangfire.AspNetCore;
using Unity;

namespace SessionControl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {      
            string configString = Configuration.GetConnectionString("redis");
            var options = ConfigurationOptions.Parse(configString);
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options);
            services.AddScoped(s => redis.GetDatabase());
            //services.AddTransient<ISqlDatabaseConnection, SqlDatabaseConnection>();
            //services.AddTransient<ISqlDatabaseWork, SqlDatabaseWork>();

            services.AddControllersWithViews();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
            });
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = "rabbitmq",
                    DispatchConsumersAsync = true
                };
                factory.UserName = "user";
                factory.Password = "password";

                var retryCount = 5;

                return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IEventBus, RabbitMqClient>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMqClient>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                return new RabbitMqClient(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubscriptionManager);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            //SQL - Hangfire Job Storage
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));

            //var unityContainer = new UnityContainer();          
            //GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(unityContainer));

            //Redis - Hangfire Job Storage
            //services.AddHangfire(configuration => configuration.UseRedisStorage(
            //    Configuration.GetConnectionString("redis"), 
            //    new RedisStorageOptions { Prefix = "{hangfire-1}:" }));

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new MyAuthorizationFilter() },
                IgnoreAntiforgeryToken = true
            });
        }

        //private void RegisterHangfireWork(IServiceCollection services)
        //{
        //    services.AddSingleton<ISqlDatabaseWork, SqlDatabaseWork>(co =>
        //    {
        //        var con = co.GetRequiredService<ISqlDatabaseConnection>();
        //        return new SqlDatabaseWork(con);
        //    });
        //    services.AddSingleton<IHangfireJob, HangfireJob>(sp =>
        //    {
        //        var sql = sp.GetRequiredService<ISqlDatabaseWork>();
        //        var redis = sp.GetRequiredService<IDatabase>();

        //        return new HangfireJob(redis, sql);
        //    });
        //}
    }
}
