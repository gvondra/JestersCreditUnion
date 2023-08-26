using System;

namespace JestersCreditUnion.Framework
{
    public interface IPaymentFactory
    {
        IPayment Create(
            string loanNumber,
            string transactionNumber,
            DateTime date);
    }
}
