using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IAddressDataFactory
    {
        Task<AddressData> Get(ISqlSettings settings, Guid id);
        Task<IEnumerable<AddressData>> GetByHash(ISqlSettings settings, byte[] hash);
    }
}
