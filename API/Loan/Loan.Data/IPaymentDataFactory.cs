using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPaymentDataFactory
    {
        Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISettings settings, DateTime date, Guid loanId, string transactionNumber);
        Task<IEnumerable<PaymentData>> GetByLoanId(ISettings settings, Guid loanId);
    }
}
