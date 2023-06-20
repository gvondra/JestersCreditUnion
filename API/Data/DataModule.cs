using Autofac;
using JestersCreditUnion.Data.Internal;

namespace JestersCreditUnion.Data
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<SqlClientProviderFactory>().As<IDbProviderFactory>();
            builder.RegisterType<AddressDataFactory>().As<IAddressDataFactory>();
            builder.RegisterType<AddressDataSaver>().As<IAddressDataSaver>();
            builder.RegisterType<EmailAddressDataFactory>().As<IEmailAddressDataFactory>();
            builder.RegisterType<EmailAddressDataSaver>().As<IEmailAddressDataSaver>();
            builder.RegisterType<LoanAgreementDataSaver>().As<ILoanAgreementDataSaver>();
            builder.RegisterType<LoanApplicationDataFactory>().As<ILoanApplicationDataFactory>();
            builder.RegisterType<LoanApplicationDataSaver>().As<ILoanApplicationDataSaver>();
            builder.RegisterType<LoanDataFactory>().As<ILoanDataFactory>();
            builder.RegisterType<LoanDataSaver>()
                .As<ILoanDataSaver>()
                .PropertiesAutowired();
            builder.RegisterType<PhoneDataFactory>().As<IPhoneDataFactory>();
            builder.RegisterType<PhoneDataSaver>().As<IPhoneDataSaver>();
        }
    }
}
