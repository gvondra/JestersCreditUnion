using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface ITransactionDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, TransactionData data, Guid? paymentId = null, short? termNumber = null);
    }
}
