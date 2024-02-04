using Autofac;
using JCU.Internal.Behaviors;

namespace JCU.Internal.DependencyInjection
{
    public class InternalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.Loan.JestersCreditUnionLoanInterfaceModule());
            builder.RegisterType<SettingsFactory>().As<ISettingsFactory>().SingleInstance();

            builder.RegisterType<LoanApplicationLoader>();
            builder.RegisterType<LoanApplicationRatingLogLoader>();
            builder.RegisterType<LoanApplicationSummaryLoader>();
            builder.RegisterType<OpenLoanSummaryLoader>();
            builder.RegisterType<WorkTaskCycleSummaryLoader>();
        }
    }
}
