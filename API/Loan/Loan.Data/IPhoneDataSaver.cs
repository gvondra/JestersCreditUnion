using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPhoneDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, PhoneData data);
    }
}
