using JestersCreditUnion.Loan.Framework.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class LoanSummaryBuilder : ILoanSummaryBuilder
    {
        public ILoanSummary BuildOpenLoanSummary(IEnumerable<IOpenLoanSummary> items)
        {
            LoanSummary loanSummary = new LoanSummary(items);
            DateTime minDueDate = DateTime.Today.AddDays(-60);
            loanSummary.Count60DaysOverdue = items.Count(i => i.NextPaymentDue <= minDueDate);
            loanSummary.MedianBalance = CalculateMedian(items);
            return loanSummary;
        }

        private static double CalculateMedian(IEnumerable<IOpenLoanSummary> items)
        {
            decimal[] balances = items.Select(i => i.Balance).Order().ToArray();
            return balances.Length switch
            {
                0 => 0.0,
                // 1 => (double)balances[0],
                _ => CalculateMedian(balances)
            };
        }

        private static double CalculateMedian(decimal[] balances)
        {
            double i = (balances.Length - 1) / 2.0;
            return ((double)balances[(int)Math.Floor(i)] + (double)balances[(int)Math.Ceiling(i)]) / 2.0;
        }
    }
}
