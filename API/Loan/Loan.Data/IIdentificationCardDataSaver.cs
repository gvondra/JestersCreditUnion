using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IIdentificationCardDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, IdentificationCardData data);
        Task Update(ISqlTransactionHandler transactionHandler, IdentificationCardData data);
    }
}
