namespace JestersCreditUnion.Loan.Data.Models
{
    public class IdentificationCardData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid IdentificationCardId { get; set; }
        [ColumnMapping] public byte[] InitializationVector { get; set; }
        [ColumnMapping] public byte[] Key { get; set; }
        [ColumnMapping] public string MasterKeyName { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
    }
}
