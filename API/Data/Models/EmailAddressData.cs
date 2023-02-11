using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
    public class EmailAddressData 
    {
        [BsonId()] public Guid EmailAddressId { get; set; }
        public string Address { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
    }
}
