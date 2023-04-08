﻿using Autofac;

namespace JCU.Internal.DependencyInjection
{
    public class InternalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());   
            builder.RegisterType<SettingsFactory>().As<ISettingsFactory>().SingleInstance();
        }
    }
}