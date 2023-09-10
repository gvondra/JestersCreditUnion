using JestersCreditUnion.CommonCore;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IEmailAddress
    {
        Guid EmailAddressId { get; }
        string Address { get; }
        DateTime CreateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler);
    }
}
