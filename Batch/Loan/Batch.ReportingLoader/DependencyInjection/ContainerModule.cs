using Autofac;
using BrassLoon.DataClient;

namespace JestersCreditUnion.Batch.ReportingLoader.DependencyInjection
{
    internal class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.Loan.JestersCreditUnionLoanInterfaceModule());

            builder.RegisterType<DataPurger>().As<IDataPurger>();
            builder.RegisterType<SqlClientProviderFactory>()
                .As<IDbProviderFactory>();
            builder.RegisterType<SettingsFactory>()
                .SingleInstance()
                .As<ISettingsFactory>();

            builder.RegisterType<LoanAgreementReporter>().As<IReporter>();
            builder.RegisterType<LoanBalanceReporter>().As<IReporter>();
            builder.RegisterType<LoanReporter>().As<IReporter>();
            builder.RegisterType<LoanStatusReporter>().As<IReporter>();
        }
    }
}
