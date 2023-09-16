using Autofac;
namespace LoanAPI
{
    public class LoanApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BrassLoon.Interface.Account.AccountInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Authorization.AuthorizationInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Config.ConfigInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Log.LogInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.WorkTask.WorkTaskInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Loan.Core.LoanCoreModule());
            builder.RegisterType<SettingsFactory>().As<ISettingsFactory>().InstancePerLifetimeScope();
        }

    }
}
