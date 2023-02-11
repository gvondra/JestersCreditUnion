using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
	public class AddressData
    {
        [BsonId()] public Guid AddressId { get; set; }
		public byte[] Hash { get; set; }
		public string Recipient { get; set; }
		public string Attention { get; set; }
		public string Delivery { get; set; }
		public string Secondary { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
    }
}
