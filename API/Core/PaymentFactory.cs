using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;

namespace JestersCreditUnion.Core
{
    public class PaymentFactory : IPaymentFactory
    {
        private static Payment Create(PaymentData data) => new Payment(data);

        public IPayment Create(string loanNumber,
            string transactionNumber,
            DateTime date)
        {
            if (string.IsNullOrEmpty(loanNumber))
                throw new ArgumentNullException(nameof(loanNumber));
            if (string.IsNullOrEmpty(transactionNumber))
                throw new ArgumentNullException(nameof(transactionNumber));
            Payment payment = Create(new PaymentData
            {
                LoanNumber = loanNumber,
                TransactionNumber = transactionNumber,
                Date = date
            });
            payment.Status = PaymentStatus.Unprocessed;
            return payment;
        }
    }
}
