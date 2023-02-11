using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
	public class LoanApplicationData 
    {
        [BsonId()] public Guid LoanApplicationId { get; set; }
		public Guid UserId { get; set; }
		public short Status { get; set; }
		public string BorrowerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)] public DateTime BorrowerBirthDate { get; set; }
		public Guid? BorrowerAddressId { get; set; }
		public Guid? BorrowerEmailAddressId { get; set; }
		public Guid? BorrowerPhoneId { get; set; }
		public string BorrowerEmployerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)] public DateTime? BorrowerEmploymentHireDate { get; set; }
		public decimal? BorrowerIncome { get; set; }
		public string CoBorrowerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)] public DateTime? CoBorrowerBirthDate { get; set; }
		public Guid? CoBorrowerAddressId { get; set; }
		public Guid? CoBorrowerEmailAddressId { get; set; }
		public Guid? CoBorrowerPhoneId { get; set; }
		public string CoBorrowerEmployerName { get; set; }
		[BsonDateTimeOptions(DateOnly = true)] public DateTime? CoBorrowerEmploymentHireDate { get; set; }
		public decimal? CoBorrowerIncome { get; set; }
		public decimal Amount { get; set; }
		public string Purpose { get; set; }
		public decimal? MortgagePayment { get; set; }
		public decimal? RentPayment { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime CreateTimestamp { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime UpdateTimestamp { get; set; }
	}
}
