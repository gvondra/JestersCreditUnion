using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IIdentificationCardDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, IdentificationCardData data);
        Task Update(ITransactionHandler transactionHandler, IdentificationCardData data);
    }
}
