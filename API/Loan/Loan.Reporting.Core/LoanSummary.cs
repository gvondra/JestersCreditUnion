using JestersCreditUnion.Loan.Framework.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class LoanSummary : ILoanSummary
    {
        public LoanSummary(IEnumerable<IOpenLoanSummary> items)
        {
            LoanCount = items.Count();
            Items = items;
        }
        public int LoanCount { get; private init; }

        public double MedianBalance { get; set; }

        public int Count60DaysOverdue { get; set; }

        public IEnumerable<IOpenLoanSummary> Items { get; private init; }
    }
}
