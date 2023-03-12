using Autofac;

namespace JCU.Internal.DependencyInjection
{
    public class ContainerFactory
    {
        private static readonly IContainer _container;

        static ContainerFactory()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new InternalModule());
            _container = builder.Build();
        }

        public static IContainer Container => _container;
    }
}
