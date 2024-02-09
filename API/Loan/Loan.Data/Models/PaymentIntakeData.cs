namespace JestersCreditUnion.Loan.Data.Models
{
    public class PaymentIntakeData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid PaymentIntakeId { get; set; }
        [ColumnMapping] public Guid LoanId { get; set; }
        [ColumnMapping] public Guid? PaymentId { get; set; }
        [ColumnMapping] public string TransactionNumber { get; set; }
        [ColumnMapping] public DateTime Date { get; set; }
        [ColumnMapping] public decimal Amount { get; set; }
        [ColumnMapping] public short Status { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
        [ColumnMapping] public string CreateUserId { get; set; }
        [ColumnMapping] public string UpdateUserId { get; set; }
    }
}
