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
            builder.RegisterType<IdentificationCardService>().As<IIdentificationCardService>();
            builder.RegisterType<LoanApplicationRatingService>().As<ILoanApplicationRatingService>();
            builder.RegisterType<LoanApplicationService>().As<ILoanApplicationService>();
            builder.RegisterType<LoanApplicationSummary>().As<ILoanApplicationSummary>();
            builder.RegisterType<LoanPaymentAmountService>().As<ILoanPaymentAmountService>();
            builder.RegisterType<LoanPaymentService>().As<ILoanPaymentService>();
            builder.RegisterType<LoanReportService>().As<ILoanReportService>();
            builder.RegisterType<LoanService>().As<ILoanService>();
            builder.RegisterType<LoanSummaryService>().As<ILoanSummaryService>();
            builder.RegisterType<LookupService>().As<ILookupService>();
            builder.RegisterType<PaymentIntakeService>().As<IPaymentIntakeService>();
            builder.RegisterType<WorkTaskConfigurationService>().As<IWorkTaskConfigurationService>();
            builder.RegisterType<WorkTaskCycleService>().As<IWorkTaskCycleService>();
        }
    }
}
