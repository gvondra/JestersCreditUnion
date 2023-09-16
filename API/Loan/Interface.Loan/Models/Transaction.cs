using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class Transaction
    {
        public Guid? TransactionId { get; set; }
        public Guid? LoanId { get; set; }
        public DateTime? Date { get; set; }
        public short? Type { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreateTimestamp { get; set; }
    }
}
