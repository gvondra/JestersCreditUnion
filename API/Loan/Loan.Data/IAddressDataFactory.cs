using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IAddressDataFactory
    {
        Task<AddressData> Get(ISqlSettings settings, Guid id);
        Task<IEnumerable<AddressData>> GetByHash(ISqlSettings settings, byte[] hash);
    }
}
