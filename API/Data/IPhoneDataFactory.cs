using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IPhoneDataFactory
    {
        Task<PhoneData> Get(ISqlSettings settings, Guid id);
        Task<PhoneData> Get(ISqlSettings settings, string number);
    }
}
