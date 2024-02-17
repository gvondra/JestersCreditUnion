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
            builder.RegisterType<Behaviors.LoanPastDueLoader>();
            builder.RegisterType<Behaviors.PaymentIntakeAdd>();
            builder.RegisterType<Behaviors.PaymentIntakeItemHoldToggler>();
            builder.RegisterType<Behaviors.PaymentIntakeItemLoader>();
            builder.RegisterType<Behaviors.PaymentIntakeItemUpdater>();
            builder.RegisterType<Behaviors.PaymentIntakeLoader>();
            builder.RegisterType<WorkTaskCycleSummaryLoader>();
        }
    }
}
