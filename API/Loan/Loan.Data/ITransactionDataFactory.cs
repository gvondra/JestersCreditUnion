using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ITransactionDataFactory
    {
        Task<IEnumerable<TransactionData>> GetByLoanId(ISqlSettings settings, Guid loanId);
    }
}
