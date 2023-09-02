using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IEmailAddressDataFactory
    {
        Task<EmailAddressData> Get(ISqlSettings settings, Guid id);
        Task<EmailAddressData> Get(ISqlSettings settings, string address);
    }
}
