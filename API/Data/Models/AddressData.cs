using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
	public class AddressData
    {
        [BsonId()] 
		public Guid AddressId { get; set; }
		[BsonRequired()]
		public byte[] Hash { get; set; }
		[BsonDefaultValue("")]
		[BsonIgnoreIfDefault()]
		public string Recipient { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string Attention { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string Delivery { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string Secondary { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string City { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string State { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string PostalCode { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
    }
}
