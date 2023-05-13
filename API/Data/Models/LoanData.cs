using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
    public class LoanData
    {
        [BsonId]
        public Guid LoanId { get; set; }
        [BsonRequired]
        public string Number { get; set; }
        [BsonRequired]
        public Guid LoanApplicationId { get; set; }
        [BsonRequired]
        public LoanAgreementData Agreement { get; set; }
        [BsonIgnoreIfNull]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime? InitialDisbursementDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime UpdateTimestamp { get; set; }
    }
}
