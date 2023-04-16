using Autofac;
using BrassLoon.RestClient;

namespace JestersCreditUnion.Interface
{
    public class JestersCreditUnionInterfaceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<Service>().As<IService>().InstancePerLifetimeScope();
            builder.RegisterType<RestUtil>().SingleInstance();
            builder.RegisterType<ExceptionService>().As<IExceptionService>();
            builder.RegisterType<LoanApplicationService>().As<ILoanApplicationService>();
            builder.RegisterType<MetricService>().As<IMetricService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<TraceService>().As<ITraceService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<WorkGroupService>().As<IWorkGroupService>();
            builder.RegisterType<WorkTaskConfigurationService>().As<IWorkTaskConfigurationService>();
            builder.RegisterType<WorkTaskService>().As<IWorkTaskService>();
            builder.RegisterType<WorkTaskStatusService>().As<IWorkTaskStatusService>();
            builder.RegisterType<WorkTaskTypeService>().As<IWorkTaskTypeService>();
        }
    }
}
