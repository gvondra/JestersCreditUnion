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
        public Guid? BorrowerAddressId { get; set; }
        public Guid? BorrowerEmailAddressId { get; set; }
        public Guid? BorrowerPhoneId { get; set; }
        public string CoBorrowerName { get; set; }
        public DateTime? CoBorrowerBirthDate { get; set; }
        public Guid? CoBorrowerAddressId { get; set; }
        public Guid? CoBorrowerEmailAddressId { get; set; }
        public Guid? CoBorrowerPhoneId { get; set; }
        public decimal? OriginalAmount { get; set; }
        public short? OriginalTerm { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public short? PaymentFrequency { get; set; }
    }
}
