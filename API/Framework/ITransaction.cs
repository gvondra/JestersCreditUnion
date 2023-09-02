using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ITransaction
    {
        Guid TransactionId { get; }
        Guid LoanId { get; }
        DateTime Date { get; }
        TransactionType Type { get; }
        decimal Amount { get; }
        bool IsNew { get; }
        DateTime CreateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler, Guid? paymentId = null, short? termNumber = null);
    }
}
