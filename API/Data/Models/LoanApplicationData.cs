using System.Collections.Generic;

namespace JestersCreditUnion.Data.Models
{
    public class LoanApplicationData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid LoanApplicationId { get; set; }
        [ColumnMapping()] public Guid UserId { get; set; }
        [ColumnMapping()] public short Status { get; set; }
        [ColumnMapping()] public DateTime ApplicationDate { get; set; }
        [ColumnMapping()] public string BorrowerName { get; set; }
        [ColumnMapping()] public DateTime BorrowerBirthDate { get; set; }
        [ColumnMapping()] public Guid? BorrowerAddressId { get; set; }
        [ColumnMapping()] public Guid? BorrowerEmailAddressId { get; set; }
        [ColumnMapping()] public Guid? BorrowerPhoneId { get; set; }
        [ColumnMapping()] public string BorrowerEmployerName { get; set; }
        [ColumnMapping()] public DateTime? BorrowerEmploymentHireDate { get; set; }
        [ColumnMapping()] public decimal? BorrowerIncome { get; set; }
        [ColumnMapping()] public string CoBorrowerName { get; set; }
        [ColumnMapping()] public DateTime? CoBorrowerBirthDate { get; set; }
        [ColumnMapping()] public Guid? CoBorrowerAddressId { get; set; }
        [ColumnMapping()] public Guid? CoBorrowerEmailAddressId { get; set; }
        [ColumnMapping()] public Guid? CoBorrowerPhoneId { get; set; }
        [ColumnMapping()] public string CoBorrowerEmployerName { get; set; }
        [ColumnMapping()] public DateTime? CoBorrowerEmploymentHireDate { get; set; }
        [ColumnMapping()] public decimal? CoBorrowerIncome { get; set; }
        [ColumnMapping()] public decimal Amount { get; set; }
        [ColumnMapping()] public string Purpose { get; set; }
        [ColumnMapping()] public decimal? MortgagePayment { get; set; }
        [ColumnMapping()] public decimal? RentPayment { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
        public List<LoanApplicationCommentData> Comments { get; set; }
        public LoanApplicationDenialData Denial { get; set; }
    }
}
