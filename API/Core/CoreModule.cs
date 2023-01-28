using Autofac;
using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework;

namespace JestersCreditUnion.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Data.DataModule());
            builder.RegisterType<Saver>().SingleInstance();
            builder.RegisterType<AddressFactory>().As<IAddressFactory>();
            builder.RegisterType<AddressSaver>().As<IAddressSaver>();
            builder.RegisterType<EmailAddressFactory>().As<IEmailAddressFactory>();
            builder.RegisterType<EmailAddressSaver>().As<IEmailAddressSaver>();
            builder.RegisterType<PhoneFactory>().As<IPhoneFactory>();
            builder.RegisterType<PhoneSaver>().As<IPhoneSaver>();
        }
    }
}
