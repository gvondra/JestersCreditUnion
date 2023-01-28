using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IEmailAddressDataFactory
    {
        Task<EmailAddressData> Get(ISqlSettings settings, Guid id);
        Task<EmailAddressData> Get(ISqlSettings settings, string address);
    }
}
