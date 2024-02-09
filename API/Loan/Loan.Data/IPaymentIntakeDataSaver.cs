using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPaymentIntakeDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, PaymentIntakeData data, string userId);
        Task Update(ITransactionHandler transactionHandler, PaymentIntakeData data, string userId);
    }
}
