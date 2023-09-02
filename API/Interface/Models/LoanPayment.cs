using System;

namespace JestersCreditUnion.Interface.Models
{
    public class LoanPayment
    {
        public Guid? PaymentId { get; set; }
        public string LoanNumber { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public short? Status { get; set; }
        public string Message { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
    }
}
