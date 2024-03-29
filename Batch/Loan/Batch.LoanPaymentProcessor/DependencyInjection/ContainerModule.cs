﻿using Autofac;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BrassLoon.Interface.Account.AccountInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Config.ConfigInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.WorkTask.WorkTaskInterfaceModule());
            builder.RegisterModule(new JestersCreditUnion.Loan.Core.LoanCoreModule());
            builder.RegisterType<LoanPaymentProcessor>().SingleInstance();
            builder.RegisterType<SettingsFactory>().SingleInstance();
        }
    }
}
