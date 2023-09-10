using Autofac;
using JestersCreditUnion.Loan.Framework;

namespace JestersCreditUnion.Loan.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Loan.Data.LoanDataModule());
            builder.RegisterType<SettingsFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AddressFactory>().As<IAddressFactory>();
            builder.RegisterType<AddressSaver>().As<IAddressSaver>();
            builder.RegisterType<AmortizationBuilder>().As<IAmortizationBuilder>();
            builder.RegisterType<EmailAddressFactory>().As<IEmailAddressFactory>();
            builder.RegisterType<EmailAddressSaver>().As<IEmailAddressSaver>();
            builder.RegisterType<KeyVault>().As<IKeyVault>();
            builder.RegisterType<LoanApplicationFactory>()
                .As<ILoanApplicationFactory>()
                .PropertiesAutowired();
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
            builder.RegisterType<LookupFactory>().As<ILookupFactory>();
            builder.RegisterType<PaymentFactory>()
                .PropertiesAutowired()
                .As<IPaymentFactory>();
            builder.RegisterType<PaymentSaver>().As<IPaymentSaver>();
            builder.RegisterType<LoanPaymentProcessor>()
                .InstancePerLifetimeScope()
                .As<ILoanPaymentProcessor>();
            builder.RegisterType<PhoneFactory>().As<IPhoneFactory>();
            builder.RegisterType<PhoneSaver>().As<IPhoneSaver>();
            builder.RegisterType<TransactionFactory>().As<ITransactionFacatory>();
            builder.RegisterType<WorkTaskTypeCodeLookup>().InstancePerLifetimeScope();
        }
    }
}
