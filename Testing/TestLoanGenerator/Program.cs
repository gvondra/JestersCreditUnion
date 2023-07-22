using Microsoft.Extensions.Configuration;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public static class Program
    {
        private static Settings _settings;
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            _settings = LoadSettings(GetConfiguration(args));
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