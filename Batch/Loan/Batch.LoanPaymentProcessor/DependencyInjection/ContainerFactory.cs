using Autofac;
using BrassLoon.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor.DependencyInjection
{
    public static class ContainerFactory
    {
        private static IContainer _container;

        public static void Initialize(
            Settings settings = null)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new ContainerModule());
            if (settings != null)
            {
                builder.RegisterInstance<Settings>(settings);
                if (!string.IsNullOrEmpty(settings.BrassLoonLogRpcBaseAddress) && settings.BrassLoonLogClientId.HasValue)
                {
                    RegisterLogging(builder, settings);
                }
            }
            _container = builder.Build();
        }

        private static void RegisterLogging(ContainerBuilder builder, Settings settings)
        {
            builder.Register<ILoggerFactory>(c => LoggerFactory.Create(b =>
            {
                b.AddBrassLoonLogger(config =>
                {
                    config.LogApiBaseAddress = settings.BrassLoonLogRpcBaseAddress;
                    config.LogDomainId = settings.LogDomainId.Value;
                    config.LogClientId = settings.BrassLoonLogClientId.Value;
                    config.LogClientSecret = settings.BrassLoonLogClientSecret;
                })
                .AddConsole();
            })).SingleInstance();
            builder.RegisterGeneric((context, types) =>
            {
                ILoggerFactory loggerFactory = context.Resolve<ILoggerFactory>();
                Type factoryType = typeof(LoggerFactoryExtensions);
                MethodInfo methodInfo = factoryType.GetMethod("CreateLogger", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(ILoggerFactory) });
                methodInfo = methodInfo.MakeGenericMethod(types);
                return methodInfo.Invoke(null, new object[] { loggerFactory });

            }).As(typeof(ILogger<>));
            builder.Register<string, ILogger>((context, categoryName) =>
            {
                ILoggerFactory loggerFactory = context.Resolve<ILoggerFactory>();
                return loggerFactory.CreateLogger(categoryName);
            });
        }

        public static ILifetimeScope BeginLifetimeScope() => _container.BeginLifetimeScope();
    }
}
