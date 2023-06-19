namespace JestersCreditUnion.Data.Models
{    
    public class PhoneData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid PhoneId { get; set; }
        [ColumnMapping()] public string Number { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
    }
}
