using System.Collections.Generic;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class OpenLoanSummary
    {
        public int LoanCount { get; set; }
        public double MedianBalance { get; set; }
        public int Count60DaysOverdue { get; set; }
        public List<OpenLoanSummaryItem> Items { get; set; }
    }
}
