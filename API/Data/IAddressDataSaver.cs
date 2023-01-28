using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IAddressDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, AddressData data);
    }
}
