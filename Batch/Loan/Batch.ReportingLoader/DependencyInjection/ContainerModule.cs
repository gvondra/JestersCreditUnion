using Autofac;
using BrassLoon.DataClient;
using BrassLoon.DataClient.MySql;

namespace JestersCreditUnion.Batch.ReportingLoader.DependencyInjection
{
    internal class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BrassLoon.Interface.Account.AccountInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Authorization.AuthorizationInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.WorkTask.WorkTaskInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.Loan.JestersCreditUnionLoanInterfaceModule());

            builder.RegisterType<DataPurger>().As<IDataPurger>();
            builder.RegisterType<MySqlProviderFactory>()
                .As<IDbProviderFactory>();
            builder.RegisterType<SettingsFactory>()
                .SingleInstance()
                .As<ISettingsFactory>();

            builder.RegisterType<LoanAgreementReporter>().As<IReporter>();
            builder.RegisterType<LoanApplicationFactReporter>().As<IReporter>();
            builder.RegisterType<LoanApplicationStatusReporter>().As<IReporter>();
            builder.RegisterType<LoanBalanceReporter>().As<IReporter>();
            builder.RegisterType<LoanReporter>().As<IReporter>();
            builder.RegisterType<LoanStatusReporter>().As<IReporter>();
            builder.RegisterType<UserReporter>().As<IReporter>();
            builder.RegisterType<WorkTaskReporter>().As<IReporter>();
        }
    }
}
