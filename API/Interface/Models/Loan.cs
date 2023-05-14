using System;

namespace JestersCreditUnion.Interface.Models
{
    public class Loan
    {
        public Guid? LoanId { get; set; }
        public string Number { get; set; }
        public Guid? LoanApplicationId { get; set; }
        public LoanAgreement Agreement { get; set; }
        public DateTime? InitialDisbursementDate { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
    }
}
