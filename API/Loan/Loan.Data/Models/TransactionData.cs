namespace JestersCreditUnion.Data.Models
{
    public class TransactionData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid TransactionId { get; set; }
        [ColumnMapping] public Guid LoanId { get; set; }
        [ColumnMapping] public DateTime Date { get; set; }
        [ColumnMapping] public short Type { get; set; }
        [ColumnMapping] public decimal Amount { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
    }
}
