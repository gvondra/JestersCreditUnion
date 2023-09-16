namespace JestersCreditUnion.Loan.Data.Models
{
    public class LoanData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid LoanId { get; set; }
        [ColumnMapping] public string Number { get; set; }
        [ColumnMapping] public Guid LoanApplicationId { get; set; }
        [ColumnMapping] public DateTime? InitialDisbursementDate { get; set; }
        [ColumnMapping] public DateTime? FirstPaymentDue { get; set; }
        [ColumnMapping] public DateTime? NextPaymentDue { get; set; }
        [ColumnMapping] public short Status { get; set; }
        [ColumnMapping] public decimal? Balance { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
        public LoanAgreementData Agreement { get; set; }
    }
}
