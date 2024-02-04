using Autofac;

namespace JestersCreditUnion.Batch.ServiceBusProcessor.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BrassLoon.Interface.Authorization.AuthorizationInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.Loan.JestersCreditUnionLoanInterfaceModule());

            builder.RegisterType<NewLoanApplicationHandler>().Keyed<IMesssageHandler>("new-ln-app");
            builder.RegisterType<ServiceBusReader>().As<IServiceBusReader>();
            builder.RegisterType<SettingsFactory>()
                .SingleInstance()
                .As<ISettingsFactory>();
        }
    }
}
