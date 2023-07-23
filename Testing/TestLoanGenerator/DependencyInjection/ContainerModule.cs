using Autofac;

namespace JestersCreditUnion.Testing.LoanGenerator.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DateGenerator>()
                .As<IDateGenerator>()
                .SingleInstance();
            builder.RegisterType<NameGenerator>()
                .As<INameGenerator>()
                .SingleInstance();
            builder.RegisterType<SettingsFactory>()
                .As<ISettingsFactory>()
                .SingleInstance();
        }
    }
}
