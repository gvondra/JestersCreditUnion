using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IPhoneDataFactory
    {
        Task<PhoneData> Get(IDataSettings settings, Guid id);
        Task<PhoneData> Get(IDataSettings settings, string number);
    }
}
