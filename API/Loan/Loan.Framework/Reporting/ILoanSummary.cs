using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface ILoanSummary
    {
        int LoanCount { get; }
        double MedianBalance { get; }
        int Count60DaysOverdue { get; }
        IEnumerable<IOpenLoanSummary> Items { get; }
    }
}
