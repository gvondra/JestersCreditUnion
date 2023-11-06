using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
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
                IEnumerable<IReporter> reporters = scope.Resolve<IEnumerable<IReporter>>();
                try
                {
                    await PurgeWorkingData(reporters);
                    await StageWorkingData(reporters);
                    await MergeWorkingDataToDestination(reporters);
#if !DEBUG
                    await PurgeWorkingData(reporters);
#endif
                }
                finally
                {
                    foreach (IReporter reporter in reporters)
                    {
                        reporter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger logger = scope.Resolve<Func<string, ILogger>>()("Program");
                logger.LogError(ex, ex.Message);
            }
            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        private static Task PurgeWorkingData(IEnumerable<IReporter> reporters)
        {
            List<Task> tasks = new List<Task>();
            foreach (IReporter reporter in reporters)
            {
                tasks.Add(reporter.PurgeWorkingData());
            }
            return Task.WhenAll(tasks);
        }

        private static async Task StageWorkingData(IEnumerable<IReporter> reporters)
        {
            List<Task> tasks;
            foreach (IGrouping<int, IReporter> reporterGroup in reporters
                .GroupBy(r => r.Order)
                .OrderBy(g => g.Key))
            {
                tasks = new List<Task>();
                foreach (IReporter reporter in reporterGroup)
                {
                    tasks.Add(Task.Run(reporter.StageWorkingData));
                }
                await Task.WhenAll(tasks);
            }
        }

        private static async Task MergeWorkingDataToDestination(IEnumerable<IReporter> reporters)
        {
            List<Task> tasks;
            foreach (IGrouping<int, IReporter> reporterGroup in reporters
                .GroupBy(r => r.Order)
                .OrderBy(g => g.Key))
            {
                tasks = new List<Task>();
                foreach (IReporter reporter in reporterGroup)
                {
                    tasks.Add(reporter.MergeWorkingDataToDestination());
                }
                await Task.WhenAll(tasks);
            }
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