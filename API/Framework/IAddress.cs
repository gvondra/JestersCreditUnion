using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
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

        Task Create(ISettings settings);
    }
}
