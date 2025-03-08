using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data
{
    public interface ILoanApplicationDataFactory
    {
        Task<IEnumerable<LoanApplicationCountData>> GetCounts(ISettings settings, DateTime minApplicationDate);
        Task<IEnumerable<LoanApplicationCloseData>> GetClosed(ISettings settings, DateTime minApplicationDate);
    }
}
