namespace JestersCreditUnion.Data.Models
{
	public class AddressData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid AddressId { get; set; }
        [ColumnMapping()] public byte[] Hash { get; set; }
        [ColumnMapping()] public string Recipient { get; set; }
        [ColumnMapping()] public string Attention { get; set; }
        [ColumnMapping()] public string Delivery { get; set; }
        [ColumnMapping()] public string Secondary { get; set; }
        [ColumnMapping()] public string City { get; set; }
        [ColumnMapping()] public string State { get; set; }
        [ColumnMapping()] public string PostalCode { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
    }
}
