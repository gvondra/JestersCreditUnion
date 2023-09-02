using System;

namespace JestersCreditUnion.Interface.Models
{
    public class LoanAgreement
    {
        public short? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? AgreementDate { get; set; }
        public string BorrowerName { get; set; }
        public DateTime? BorrowerBirthDate { get; set; }
        public Address BorrowerAddress { get; set; }
        public string BorrowerEmailAddress { get; set; }
        public string BorrowerPhone { get; set; }
        public string CoBorrowerName { get; set; }
        public DateTime? CoBorrowerBirthDate { get; set; }
        public Address CoBorrowerAddress { get; set; }
        public string CoBorrowerEmailAddress { get; set; }
        public string CoBorrowerPhone { get; set; }
        public decimal? OriginalAmount { get; set; }
        public short? OriginalTerm { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public short? PaymentFrequency { get; set; }
    }
}
