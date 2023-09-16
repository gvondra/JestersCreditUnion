using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class Loan
    {
        public Guid? LoanId { get; set; }
        public string Number { get; set; }
        public Guid? LoanApplicationId { get; set; }
        public LoanAgreement Agreement { get; set; }
        public DateTime? InitialDisbursementDate { get; set; }
        public DateTime? FirstPaymentDue { get; set; }
        public DateTime? NextPaymentDue { get; set; }
        public short? Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
    }
}
