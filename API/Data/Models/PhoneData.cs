using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{    
    public class PhoneData 
    {
        [BsonId()] public Guid PhoneId { get; set; }
        public string Number { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
    }
}
