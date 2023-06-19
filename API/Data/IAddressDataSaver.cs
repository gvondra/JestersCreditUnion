using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IAddressDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, AddressData data);
    }
}
