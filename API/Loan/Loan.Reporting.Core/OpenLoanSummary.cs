using JestersCreditUnion.Loan.Framework.Reporting;
using JestersCreditUnion.Loan.Reporting.Data.Models;
using System;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class OpenLoanSummary : IOpenLoanSummary
    {
        private readonly OpenLoanSummaryData _data;

        public OpenLoanSummary(OpenLoanSummaryData data)
        {
            _data = data;
        }

        public string Number => _data.Number;

        public decimal Balance => _data.Balance;

        public DateTime NextPaymentDue => _data.NextPaymentDue;
    }
}
