using System;

namespace JestersCreditUnion.Framework
{
    public interface ILoan
    {
        public Guid LoanId { get; }
        public string Number { get; }
        public Guid LoanApplicationId { get; }
        public ILoanAgreement Agreement { get; }
        public DateTime? InitialDisbursementDate { get; set; }
        public DateTime CreateTimestamp { get; }
        public DateTime UpdateTimestamp { get; }
    }
}
