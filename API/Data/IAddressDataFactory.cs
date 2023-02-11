using JestersCreditUnion.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IAddressDataFactory
    {
        Task<AddressData> Get(IDataSettings settings, Guid id);
        Task<IEnumerable<AddressData>> GetByHash(IDataSettings settings, byte[] hash);
    }
}
