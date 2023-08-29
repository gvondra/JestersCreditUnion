using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IPaymentFactory
    {
        IPayment Create(
            string loanNumber,
            string transactionNumber,
            DateTime date);

        Task<IEnumerable<IPayment>> GetByStatus(ISettings settings, PaymentStatus status);
    }
}
