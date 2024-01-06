﻿using Autofac;
using JestersCreditUnion.Loan.Framework.Reporting;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class LoanReportingCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Loan.Reporting.Data.LoanReportingDataModule());

            builder.RegisterType<LoanApplicationFactory>().As<ILoanApplicationFactory>();
        }
    }
}
