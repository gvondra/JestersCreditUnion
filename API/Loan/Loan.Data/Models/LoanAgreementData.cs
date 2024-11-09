namespace JestersCreditUnion.Loan.Data.Models
{
    public class LoanAgreementData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid LoanId { get; set; }
        [ColumnMapping] public short Status { get; set; }
        [ColumnMapping] public DateTime CreateDate { get; set; }
        [ColumnMapping] public DateTime? AgreementDate { get; set; }
        [ColumnMapping] public string BorrowerName { get; set; }
        [ColumnMapping] public DateTime BorrowerBirthDate { get; set; }
        [ColumnMapping] public Guid? BorrowerAddressId { get; set; }
        [ColumnMapping] public Guid? BorrowerEmailAddressId { get; set; }
        [ColumnMapping] public Guid? BorrowerPhoneId { get; set; }
        [ColumnMapping] public string CoBorrowerName { get; set; }
        [ColumnMapping] public DateTime? CoBorrowerBirthDate { get; set; }
        [ColumnMapping] public Guid? CoBorrowerAddressId { get; set; }
        [ColumnMapping] public Guid? CoBorrowerEmailAddressId { get; set; }
        [ColumnMapping] public Guid? CoBorrowerPhoneId { get; set; }
        [ColumnMapping] public decimal OriginalAmount { get; set; }
        [ColumnMapping] public short OriginalTerm { get; set; }
        [ColumnMapping] public decimal InterestRate { get; set; }
        [ColumnMapping] public decimal PaymentAmount { get; set; }
        [ColumnMapping] public short PaymentFrequency { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
