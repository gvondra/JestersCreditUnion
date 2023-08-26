using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IPaymentDataSaver
    {
        Task Save(ITransactionHandler transactionHandler, IEnumerable<PaymentData> payments);
    }
}
