using Autofac;

namespace JestersCreditUnion.Batch.ServiceBusProcessor.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<NewLoanApplicationHandler>().Keyed<IMesssageHandler>("new-ln-app");
            builder.RegisterType<ServiceBusReader>().As<IServiceBusReader>();
            builder.RegisterType<SettingsFactory>()
                .SingleInstance()
                .As<ISettingsFactory>();
        }
    }
}
