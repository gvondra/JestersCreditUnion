﻿using Autofac;
namespace API
{
    public class APIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BrassLoon.Interface.Account.AccountInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Authorization.AuthorizationInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Log.LogInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.WorkTask.WorkTaskInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Core.CoreModule());
            builder.RegisterType<SettingsFactory>().As<ISettingsFactory>().InstancePerLifetimeScope();
        }
    }
}
