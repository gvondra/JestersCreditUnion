using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IPaymentDataFactory
    {
        Task<IEnumerable<PaymentData>> GetByStatus(ISqlSettings settings, short status);
        Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISqlSettings settings, DateTime date, Guid loanId, string transactionNumber);
    }
}
