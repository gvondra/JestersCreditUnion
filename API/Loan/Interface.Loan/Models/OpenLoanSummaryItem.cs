using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class OpenLoanSummaryItem
    {
        public string Number { get; set; }
        public decimal Balance { get; set;  }
        public DateTime NextPaymentDue { get; set; }
    }
}
