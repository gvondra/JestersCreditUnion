﻿using Autofac;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public static class Program
    {
        private static Settings _settings;        
        public static async Task Main(string[] args)
        {
            _settings = LoadSettings(GetConfiguration(args));
            ILogger logger = CreateLogger();
            try
            {
                await NameGenerator.Initialize();
                DependencyInjection.ContainerFactory.Initialize(_settings, logger);
                await StartLoanApplicationGeneration();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fatal error encountered");
            }
        }

        private static async Task StartLoanApplicationGeneration()
        {
            using ILifetimeScope scope = DependencyInjection.ContainerFactory.BeginLifetimeScope();
            Settings settings = scope.Resolve<Settings>();
            using LoanProcess loanProcess = scope.Resolve<LoanProcess>();
            using LoanTaskProcess loanTaskProcess = scope.Resolve<LoanTaskProcess>();
            using DisburseLoanProcess disburseLoanProcess = scope.Resolve<DisburseLoanProcess>();
            loanProcess.AddObserver(loanTaskProcess);
            loanProcess.AddObserver(disburseLoanProcess);

            using ILoanApplicationTaskProcess loanApplicationTaskProcess = scope.Resolve<ILoanApplicationTaskProcess>();
            using INewLoanApplicationTaskProcess newLoanApplicationTaskProcess = scope.Resolve<INewLoanApplicationTaskProcess>();
            loanApplicationTaskProcess.AddObserver(newLoanApplicationTaskProcess);
            newLoanApplicationTaskProcess.AddObserver(loanApplicationTaskProcess);
            newLoanApplicationTaskProcess.AddObserver(loanProcess);

            ILoanApplicationProcess process = scope.Resolve<ILoanApplicationProcess>();
            process.AddObserver(loanApplicationTaskProcess);
            await process.GenerateLoanApplications();

            while (newLoanApplicationTaskProcess.Count < settings.LoanApplicationCount)
            {
                await Task.Delay(12000);
            }

            loanApplicationTaskProcess.WaitForProcessExit();
            newLoanApplicationTaskProcess.WaitForProcessExit();
            loanProcess.WaitForProcessExit();
            disburseLoanProcess.WaitForProcessExit();
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

        private static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}