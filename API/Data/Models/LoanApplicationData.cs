using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Data.Models
{
	public class LoanApplicationData 
    {
        [BsonId()] 
		public Guid LoanApplicationId { get; set; }
		[BsonRequired()]
		public Guid UserId { get; set; }
		[BsonRequired()]
		[BsonDefaultValue(0)]
		public short Status { get; set; }
        [BsonRequired()]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime ApplicationDate { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        [BsonIgnoreIfNull()]
        public string BorrowerName { get; set; }
		[BsonRequired()]
		[BsonDateTimeOptions(DateOnly = true)]
		public DateTime BorrowerBirthDate { get; set; }
        [BsonIgnoreIfNull()]
        public Guid? BorrowerAddressId { get; set; }
        [BsonIgnoreIfNull()]
        public Guid? BorrowerEmailAddressId { get; set; }
        [BsonIgnoreIfNull()]
        public Guid? BorrowerPhoneId { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string BorrowerEmployerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)]
        [BsonIgnoreIfNull()]
        public DateTime? BorrowerEmploymentHireDate { get; set; }
        [BsonIgnoreIfNull()]
        public decimal? BorrowerIncome { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string CoBorrowerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)]
        [BsonIgnoreIfNull()]
        public DateTime? CoBorrowerBirthDate { get; set; }
        [BsonIgnoreIfNull()]
        public Guid? CoBorrowerAddressId { get; set; }
        [BsonIgnoreIfNull()]
        public Guid? CoBorrowerEmailAddressId { get; set; }
        [BsonIgnoreIfNull()]
        public Guid? CoBorrowerPhoneId { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        public string CoBorrowerEmployerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)]
        [BsonIgnoreIfNull()]
        public DateTime? CoBorrowerEmploymentHireDate { get; set; }
        [BsonIgnoreIfNull()]
        public decimal? CoBorrowerIncome { get; set; }
		[BsonRequired()]
		public decimal Amount { get; set; }
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault()]
        [BsonIgnoreIfNull()]
        public string Purpose { get; set; }
        [BsonIgnoreIfNull()]
        public decimal? MortgagePayment { get; set; }
		[BsonIgnoreIfNull()]
		public decimal? RentPayment { get; set; }
        [BsonIgnoreIfNull]
        public List<LoanApplicationCommentData> Comments { get; set; }
        [BsonIgnoreIfNull()]
        public LoanApplicationDenialData Denial { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime UpdateTimestamp { get; set; }
	}
}
