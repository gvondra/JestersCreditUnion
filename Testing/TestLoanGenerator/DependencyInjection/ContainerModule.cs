using Autofac;

namespace JestersCreditUnion.Testing.LoanGenerator.DependencyInjection
{
    public class ContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new JestersCreditUnion.Interface.JestersCreditUnionInterfaceModule());
            builder.RegisterType<DateGenerator>()
                .As<IDateGenerator>()
                .SingleInstance();
            builder.RegisterType<DisburseLoanProcess>();
            builder.RegisterType<LoanApplicationFileWriter>()
                .As<ILoanApplicationFileWriter>();
            builder.RegisterType<LoanApplicationGenerator>()
                .As<ILoanApplicationGenerator>();
            builder.RegisterType<LoanApplicationProcess>()
                .As<ILoanApplicationProcess>();
            builder.RegisterType<LoanApplicationTaskProcess>();
            builder.RegisterType<LoanProcess>();
            builder.RegisterType<LoanTaskProcess>();
            builder.RegisterType<NameGenerator>()
                .As<INameGenerator>()
                .SingleInstance();
            builder.RegisterType<SettingsFactory>()
                .As<ISettingsFactory>()
                .SingleInstance();
        }
    }
}
