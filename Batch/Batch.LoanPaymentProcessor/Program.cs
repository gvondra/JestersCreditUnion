using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Settings settings = LoadSettings(GetConfiguration(args));
            DependencyInjection.ContainerFactory.Initialize(settings);
            using ILifetimeScope scope = DependencyInjection.ContainerFactory.BeginLifetimeScope();
            try
            {
                LoanPaymentProcessor processor = scope.Resolve<LoanPaymentProcessor>();
                await processor.Process();
            }
            catch (Exception ex)
            {
                ILogger logger = scope.Resolve<Func<string, ILogger>>()("Program");
                logger.LogError(ex, ex.Message);
            }
            await Task.Delay(TimeSpan.FromSeconds(5));
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