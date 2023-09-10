using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPaymentDataSaver
    {
        Task Save(ITransactionHandler transactionHandler, IEnumerable<PaymentData> payments);
        Task Update(ITransactionHandler transactionHandler, PaymentData data);
    }
}
