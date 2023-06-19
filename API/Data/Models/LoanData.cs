namespace JestersCreditUnion.Data.Models
{
    public class LoanData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid LoanId { get; set; }
        [ColumnMapping()] public string Number { get; set; }
        [ColumnMapping()] public Guid LoanApplicationId { get; set; }
        [ColumnMapping()] public LoanAgreementData Agreement { get; set; }
        [ColumnMapping()] public DateTime? InitialDisbursementDate { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
