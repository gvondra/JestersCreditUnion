using Autofac;
using JestersCreditUnion.Loan.Data.Internal;

namespace JestersCreditUnion.Loan.Data
{
    public class LoanDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<SqlClientProviderFactory>()
                .As<IDbProviderFactory>();
            builder.RegisterType<IdentificationCardDataSaver>().As<IIdentificationCardDataSaver>();
            builder.RegisterType<LoanAgreementDataSaver>().As<ILoanAgreementDataSaver>();
            builder.RegisterType<LoanApplicationDataFactory>().As<ILoanApplicationDataFactory>();
            builder.RegisterType<LoanApplicationDataSaver>().As<ILoanApplicationDataSaver>();
            builder.RegisterType<LoanDataFactory>().As<ILoanDataFactory>();
            builder.RegisterType<LoanDataSaver>()
                .PropertiesAutowired()
                .As<ILoanDataSaver>();
            builder.RegisterType<PaymentDataFactory>().As<IPaymentIntakeDataFactory>();
            builder.RegisterType<PaymentDataSaver>().As<IPaymentDataSaver>();
            builder.RegisterType<PaymentIntakeDataFactory>().As<IPaymentIntakeDataFactory>();
            builder.RegisterType<PaymentIntakeDataSaver>().As<IPaymentIntakeDataSaver>();
            builder.RegisterType<RatingDataFactory>().As<IRatingDataFactory>();
            builder.RegisterType<RatingDataSaver>().As<IRatingDataSaver>();
            builder.RegisterType<TransactionDataFactory>().As<ITransactionDataFactory>();
            builder.RegisterType<TransactionDataSaver>().As<ITransactionDataSaver>();
        }
    }
}
