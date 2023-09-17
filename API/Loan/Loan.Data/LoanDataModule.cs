﻿using Autofac;
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
            builder.RegisterType<AddressDataFactory>().As<IAddressDataFactory>();
            builder.RegisterType<AddressDataSaver>().As<IAddressDataSaver>();
            builder.RegisterType<EmailAddressDataFactory>().As<IEmailAddressDataFactory>();
            builder.RegisterType<EmailAddressDataSaver>().As<IEmailAddressDataSaver>();
            builder.RegisterType<IdentificationCardDataSaver>().As<IIdentificationCardDataSaver>();
            builder.RegisterType<LoanAgreementDataSaver>().As<ILoanAgreementDataSaver>();
            builder.RegisterType<LoanApplicationDataFactory>().As<ILoanApplicationDataFactory>();
            builder.RegisterType<LoanApplicationDataSaver>().As<ILoanApplicationDataSaver>();
            builder.RegisterType<LoanDataFactory>().As<ILoanDataFactory>();
            builder.RegisterType<LoanDataSaver>()
                .PropertiesAutowired()
                .As<ILoanDataSaver>();
            builder.RegisterType<PaymentDataFactory>().As<IPaymentDataFactory>();
            builder.RegisterType<PaymentDataSaver>().As<IPaymentDataSaver>();
            builder.RegisterType<PhoneDataFactory>().As<IPhoneDataFactory>();
            builder.RegisterType<PhoneDataSaver>().As<IPhoneDataSaver>();
            builder.RegisterType<TransactionDataFactory>().As<ITransactionDataFactory>();
            builder.RegisterType<TransactionDataSaver>().As<ITransactionDataSaver>();
        }
    }
}