using Autofac;
using JestersCreditUnion.Framework;

namespace JestersCreditUnion.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Data.DataModule());
            builder.RegisterType<SettingsFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AddressFactory>().As<IAddressFactory>();
            builder.RegisterType<EmailAddressFactory>().As<IEmailAddressFactory>();
            builder.RegisterType<LoanApplicationFactory>()
                .As<ILoanApplicationFactory>()
                .PropertiesAutowired();
            builder.RegisterType<LoanApplicationSaver>().As<ILoanApplicationSaver>();
            builder.RegisterType<LookupFactory>().As<ILookupFactory>();
            builder.RegisterType<PhoneFactory>().As<IPhoneFactory>();
            builder.RegisterType<WorkTaskTypeCodeLookup>().InstancePerLifetimeScope();
        }
    }
}
