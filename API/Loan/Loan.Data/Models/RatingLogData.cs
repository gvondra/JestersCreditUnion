namespace JestersCreditUnion.Loan.Data.Models
{
    public class RatingLogData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid RatingLogId { get; set; }
        [ColumnMapping] public Guid RatingId { get; set; }
        [ColumnMapping] public double? Value { get; set; }
        [ColumnMapping] public string Description { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
    }
}
