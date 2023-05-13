using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
    public class LoanAgreementData
    {
        [BsonRequired]
        [BsonDefaultValue(0)]
        public short Status { get; set; }
        [BsonRequired]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime CreateDate { get; set; }
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime? AgreementDate { get; set; }
        [BsonRequired]
        public string BorrowerName { get; set; }
        [BsonRequired]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime BorrowerBirthDate { get; set; }
        [BsonIgnoreIfNull]
        public Guid? BorrowerAddressId { get; set; }
        [BsonIgnoreIfNull]
        public Guid? BorrowerEmailAddressId { get; set; }
        [BsonIgnoreIfNull]
        public Guid? BorrowerPhoneId { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault]
        public string CoBorrowerName { get; set; }
        [BsonDateTimeOptions(DateOnly = true)]
        [BsonIgnoreIfNull]
        public DateTime? CoBorrowerBirthDate { get; set; }
        [BsonIgnoreIfNull]
        public Guid? CoBorrowerAddressId { get; set; }
        [BsonIgnoreIfNull]
        public Guid? CoBorrowerEmailAddressId { get; set; }
        [BsonIgnoreIfNull]
        public Guid? CoBorrowerPhoneId { get; set; }
        [BsonRequired]
        public decimal OriginalAmount { get; set; }
        [BsonRequired]
        public short OriginalTerm { get; set; }
        [BsonRequired]
        public decimal InterestRate { get; set; }
        [BsonRequired]
        public decimal PaymentAmount { get; set; }
        [BsonRequired]
        public short PaymentFrequency { get; set; }
    }
}
