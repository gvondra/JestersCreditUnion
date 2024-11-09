using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class PaymentIntake
    {
        public Guid? PaymentIntakeId { get; set; }
        public Guid? LoanId { get; set; }
        public Guid? PaymentId { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public short? Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public string CreateUserId { get; set; }
        public string UpdateUserId { get; set; }
        public Loan Loan { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
    }
}
