using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IEmailAddressFactory
    {
        IEmailAddress Create(string address);
        Task<IEmailAddress> Get(ISettings settings, Guid id);
        Task<IEmailAddress> Get(ISettings settings, string address);
    }
}
