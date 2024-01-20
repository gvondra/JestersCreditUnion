using Autofac;
using JestersCreditUnion.Loan.Framework;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Loan.Data.LoanDataModule());
            builder.RegisterModule(new BrassLoon.Interface.Address.AddressInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());
            builder.RegisterType<SettingsFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Address2Factory>()
                .As<IAddressFactory>()
                .Keyed<IAddressFactory>("v2");
            builder.RegisterType<AmortizationBuilder>().As<IAmortizationBuilder>();
            builder.RegisterType<KeyVault>().As<IKeyVault>();
            builder.RegisterType<LoanApplicationFactory>()
                .As<ILoanApplicationFactory>()
                .PropertiesAutowired();
            builder.RegisterType<LoanApplicationRaterFactory>().As<ILoanApplicationRaterFactory>();
            builder.RegisterType<LoanApplicationSaver>().As<ILoanApplicationSaver>();
            builder.RegisterType<LoanDisburser>().As<ILoanDisburser>();
            builder.RegisterType<LoanFactory>()
                .As<ILoanFactory>()
                .PropertiesAutowired();
            builder.RegisterType<LoanPaymentAmountCalculator>()
                .As<ILoanPaymentAmountCalculator>()
                .SingleInstance();
            builder.RegisterType<LoanNumberGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<LoanSaver>().As<ILoanSaver>();
            builder.RegisterType<LoanPaymentProcessor>()
                .InstancePerLifetimeScope()
                .As<ILoanPaymentProcessor>();
            builder.RegisterType<LookupFactory>().As<ILookupFactory>();
            builder.RegisterType<PaymentFactory>()
                .PropertiesAutowired()
                .As<IPaymentFactory>();
            builder.RegisterType<PaymentSaver>().As<IPaymentSaver>();
            builder.RegisterType<RatingFactory>().As<IRatingFactory>();
            builder.RegisterType<RatingSaver>().As<IRatingSaver>();
            builder.RegisterType<TransactionFactory>().As<ITransactionFacatory>();
            builder.RegisterType<WorkTaskTypeCodeLookup>()
                .InstancePerLifetimeScope()
                .AsSelf()
                .As<IWorkTaskTypeCodeLookup>();
        }
    }
}
