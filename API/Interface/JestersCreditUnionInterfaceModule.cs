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
            builder.RegisterType<ExceptionService>().As<IExceptionService>();
            builder.RegisterType<MetricService>().As<IMetricService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<TraceService>().As<ITraceService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<WorkTaskStatusService>().As<IWorkTaskStatusService>();
            builder.RegisterType<WorkTaskTypeService>().As<IWorkTaskTypeService>();
        }
    }
}
