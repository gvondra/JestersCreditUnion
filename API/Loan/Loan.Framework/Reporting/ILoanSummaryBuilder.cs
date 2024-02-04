using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface ILoanSummaryBuilder
    {
        ILoanSummary BuildOpenLoanSummary(IEnumerable<IOpenLoanSummary> items);
    }
}
