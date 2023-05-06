using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
    public class LoanApplicationDenialData
    {
        [BsonRequired]
        [BsonDefaultValue(0)]
        public short Reason { get; set; }
        [BsonRequired]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }
        [BsonRequired]
        public Guid UserId { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string Text { get; set; }
    }
}
