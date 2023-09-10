using JestersCreditUnion.CommonCore;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IAddress
    {
        Guid AddressId { get; }
        byte[] Hash { get; }
        string Recipient { get; }
        string Attention { get; }
        string Delivery { get; }
        string Secondary { get; }
        string City { get; }
        string State { get; }
        string PostalCode { get; }
        DateTime CreateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler);
    }
}
