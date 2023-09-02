using Autofac;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Core.CoreModule());
            builder.RegisterType<LoanPaymentProcessor>().SingleInstance();
            builder.RegisterType<SettingsFactory>().SingleInstance();
        }
    }
}
