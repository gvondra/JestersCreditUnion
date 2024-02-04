using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface ILoanSummaryFactory
    {
        Task<IEnumerable<IOpenLoanSummary>> Get(ISettings settings);
    }
}
