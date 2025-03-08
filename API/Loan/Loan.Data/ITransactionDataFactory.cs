using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface ITransactionDataFactory
    {
        Task<IEnumerable<TransactionData>> GetByLoanId(ISettings settings, Guid loanId);
    }
}
