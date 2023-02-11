using Autofac;
using JestersCreditUnion.Data.Internal;

namespace JestersCreditUnion.Data
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<MongoClientFactory>().As<IMongoClientFactory>().SingleInstance();
            builder.RegisterType<AddressDataFactory>().As<IAddressDataFactory>();
            builder.RegisterType<AddressDataSaver>().As<IAddressDataSaver>();
            builder.RegisterType<EmailAddressDataFactory>().As<IEmailAddressDataFactory>();
            builder.RegisterType<EmailAddressDataSaver>().As<IEmailAddressDataSaver>();
            builder.RegisterType<LoanApplicationDataSaver>().As<ILoanApplicationDataSaver>();
            builder.RegisterType<PhoneDataFactory>().As<IPhoneDataFactory>();
            builder.RegisterType<PhoneDataSaver>().As<IPhoneDataSaver>();
        }
    }
}
