using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data
{
    public interface ILoanApplicationDataFactory
    {
        Task<IEnumerable<LoanApplicationCountData>> GetCounts(ISqlSettings settings, DateTime minApplicationDate);
        Task<IEnumerable<LoanApplicationCloseData>> GetClosed(ISqlSettings settings, DateTime minApplicationDate);
    }
}
