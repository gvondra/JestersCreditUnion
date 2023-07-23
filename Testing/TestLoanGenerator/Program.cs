using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public static class Program
    {
        private static Settings _settings;
        public static async Task Main(string[] args)
        {
            _settings = LoadSettings(GetConfiguration(args));
            await NameGenerator.Initialize();
            DependencyInjection.ContainerFactory.Initialize(_settings);
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