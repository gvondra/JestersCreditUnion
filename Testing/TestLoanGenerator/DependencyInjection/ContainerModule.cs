using Autofac;

namespace JestersCreditUnion.Testing.LoanGenerator.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<SettingsFactory>().As<ISettingsFactory>();
        }
    }
}
