using Autofac;
using JestersCreditUnion.Loan.Reporting.Data.Internal;

namespace JestersCreditUnion.Loan.Reporting.Data
{
    public class LoanReportingDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<MySqlProviderFactory>()
                .As<IDbProviderFactory>();
            builder.RegisterType<LoanApplicationDataFactory>().As<ILoanApplicationDataFactory>();
            builder.RegisterType<LoanBalanceDataFactory>().As<ILoanBalanceDataFactory>();
            builder.RegisterType<OpenLoanSummaryDataFactory>().As<IOpenLoanSummaryDataFactory>();
            builder.RegisterType<WorkTaskCycleSummaryDataFactory>().As<IWorkTaskCycleSummaryDataFactory>();
        }
    }
}
