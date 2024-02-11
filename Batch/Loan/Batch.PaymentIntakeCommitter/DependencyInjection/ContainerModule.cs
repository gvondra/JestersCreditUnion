using Autofac;

namespace JestersCreditUnion.Batch.PaymentIntakeCommitter.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BrassLoon.Interface.Authorization.AuthorizationInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.Loan.JestersCreditUnionLoanInterfaceModule());
            builder.RegisterType<PaymentIntakeProcessor>();
            builder.RegisterType<SettingsFactory>()
                .SingleInstance()
                .As<ISettingsFactory>();
        }
    }
}
