using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPaymentDataFactory
    {
        Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISqlSettings settings, DateTime date, Guid loanId, string transactionNumber);
        Task<IEnumerable<PaymentData>> GetByLoanId(ISqlSettings settings, Guid loanId);
    }
}
