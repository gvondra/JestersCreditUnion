using Autofac;
using BrassLoon.RestClient;

namespace JestersCreditUnion.Interface
{
    public class JestersCreditUnionInterfaceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<Service>().As<IService>().SingleInstance();
            builder.RegisterType<RestUtil>().SingleInstance();
            builder.RegisterType<TokenService>().As<ITokenService>();
        }
    }
}
