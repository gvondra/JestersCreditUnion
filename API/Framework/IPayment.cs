using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IPayment
    {
        Guid PaymentId { get; }
        string LoanNumber { get; }
        string TransactionNumber { get; }
        DateTime Date { get; }
        decimal Amount { get; set; }
        PaymentStatus Status { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }
        List<ITransaction> Transactions { get; }

        Task Update(ITransactionHandler transactionHandler);
    }
}
