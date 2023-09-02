using JestersCreditUnion.CommonCore;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IPhone
    {
        Guid PhoneId { get; }
        string Number { get; }
        DateTime CreateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler);
    }
}
