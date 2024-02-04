using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Settings settings = LoadSettings(GetConfiguration(args));
            DependencyInjection.ContainerFactory.Initialize(settings);
            ILifetimeScope scope = DependencyInjection.ContainerFactory.BeginLifetimeScope();
            try
            {
                List<IServiceBusReader> readers = await Start(settings);
                await Task.Delay(TimeSpan.FromHours(settings.RunHours ?? 0.25));
                await Stop(readers);
            }
            catch (Exception ex)
            {
                ILogger logger = scope.Resolve<Func<string, ILogger>>()("Program");
                logger.LogError(ex, ex.Message);
            }
        }

        private static async Task<List<IServiceBusReader>> Start(Settings settings)
        {
            ILifetimeScope scope = DependencyInjection.ContainerFactory.BeginLifetimeScope();
            List<IServiceBusReader> readers = new List<IServiceBusReader>();
            readers.Add(
                await Start(scope, "new-ln-app", settings.ServiceBusNewLoanAppQueue));
            return readers;
        }

        private static async Task<IServiceBusReader> Start(ILifetimeScope scope, string key, string queue)
        {
            IMesssageHandler handler = scope.ResolveKeyed<IMesssageHandler>(key);
            IServiceBusReader reader = scope.Resolve<IServiceBusReader>();
            await reader.StartProcessing(queue, handler);
            return reader;
        }

        private static Task Stop(List<IServiceBusReader> readers)
        {
            Task[] tasks = new Task[readers.Count];
            for (int i = 0; i < readers.Count; i += 1)
            {
                tasks[i] = readers[i].StopProcessing();
            }
            return Task.WhenAll(tasks);
        }

        private static Settings LoadSettings(IConfiguration configuration)
        {
            Settings settings = new Settings();
            ConfigurationBinder.Bind(configuration, settings);
            return settings;
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .AddCommandLine(args);
            return builder.Build();
        }
    }
}
