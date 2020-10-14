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
using HangfireWorker;
using Unity;
using Autofac;

namespace HangfireServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Hangfire server.");
            GlobalConfiguration.Configuration.UseSqlServerStorage("Server = database; Database = HangfireDatabase; User = sa; Password = Pa&&word2020;");

            var hostBuilder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
            });
            var builder = new ContainerBuilder();
            builder.RegisterType<SqlDatabaseConnection>().As<ISqlDatabaseConnection>().InstancePerLifetimeScope();
            builder.RegisterType<SqlDatabaseWork>().As<ISqlDatabaseWork>().InstancePerBackgroundJob();
            builder.RegisterType<HangfireJob>().AsSelf().InstancePerBackgroundJob();
            GlobalConfiguration.Configuration.UseAutofacActivator(builder.Build());

            using (var server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {

                WorkerCount = Environment.ProcessorCount * 5
            }))
            {
                await hostBuilder.RunConsoleAsync();
            }
        }
    }
}
