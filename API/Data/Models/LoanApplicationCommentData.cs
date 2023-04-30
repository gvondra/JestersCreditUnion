using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
    public class LoanApplicationCommentData
    {
        [BsonId()]
        public Guid LoanApplicationCommentId { get; set; }
        [BsonRequired()]
        public Guid UserId { get; set; }
        [BsonRequired()]
        public bool IsInternal { get; set; } = true;
        [BsonRequired]
        public string Text { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
    }
}
