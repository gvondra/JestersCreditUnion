using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IAddressDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, AddressData data);
    }
}
