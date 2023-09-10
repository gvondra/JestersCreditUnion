namespace JestersCreditUnion.Data.Models
{
    public class LoanApplicationDenialData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid LoanApplicationId { get; set; }
        [ColumnMapping()] public short Reason { get; set; }
        [ColumnMapping()] public DateTime Date { get; set; }
        [ColumnMapping()] public Guid UserId { get; set; }
        [ColumnMapping()] public string Text { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
