using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IEmailAddressDataFactory
    {
        Task<EmailAddressData> Get(IDataSettings settings, Guid id);
        Task<EmailAddressData> Get(IDataSettings settings, string address);
    }
}
