using Autofac;

namespace JestersCreditUnion.Testing.LoanGenerator.DependencyInjection
{
    public static class ContainerFactory
    {
        private static IContainer _container;

        static ContainerFactory()
        {
            Initialize();
        }

        public static void Initialize(Settings settings = null)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new ContainerModule());
            if (settings != null)
            {
                builder.RegisterInstance<Settings>(settings);
            }
            _container = builder.Build();
        }
        
        public static ILifetimeScope BeginLifetimeScope() => _container.BeginLifetimeScope();
    }
}
