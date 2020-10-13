using Hangfire;
using HangfireWorker.StorageConnection;
using HangfireWorker.StorageWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;

namespace HangfireServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Hangfire server.");
            GlobalConfiguration.Configuration.UseSqlServerStorage("Server = database; Database = HangfireDatabase; User = sa; Password = Pa&&word2020;");
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);

            var configuration = builder.Build();

            var hostBuilder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                string configString = configuration.GetConnectionString("redis");
                var options = ConfigurationOptions.Parse(configString);
                IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options);
                services.AddScoped(s => redis.GetDatabase());
                services.AddTransient<ISqlDatabaseConnection, SqlDatabaseConnection>();
                services.AddTransient<ISqlDatabaseWork, SqlDatabaseWork>();
            });
            
            using (var server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {

                WorkerCount = Environment.ProcessorCount * 1
            }))
            {
                await hostBuilder.RunConsoleAsync();
            }
        }
    }
}
