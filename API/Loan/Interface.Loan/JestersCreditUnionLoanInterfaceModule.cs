using Autofac;
using BrassLoon.RestClient;

namespace JestersCreditUnion.Interface.Loan
{
    public class JestersCreditUnionLoanInterfaceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<Service>().As<IService>().InstancePerLifetimeScope();
            builder.RegisterType<RestUtil>().SingleInstance();
            builder.RegisterType<AmortizationService>().As<IAmortizationService>();
        }
    }
}
