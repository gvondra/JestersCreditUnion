using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface ILoanApplicationFactory
    {
        Task<IEnumerable<LoanApplicationSummaryItem>> GetLoanApplicationSummary(ISettings settings, DateTime minAppliationDate);
    }
}
