using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IEmailAddress
    {
        Guid EmailAddressId { get; }
        string Address { get; }
        DateTime CreateTimestamp { get; }

        Task Create(ISettings settings);
    }
}
