using System;

namespace JestersCreditUnion.Interface.Models
{
    public class LoanApplication
    {
        public Guid? LoanApplicationId { get; set; }
        public short? Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string BorrowerName { get; set; }
        public DateTime? BorrowerBirthDate { get; set; }
        public Address BorrowerAddress { get; set; }
        public string BorrowerEmailAddress { get; set; }
        public string BorrowerPhone { get; set; }
        public string BorrowerEmployerName { get; set; }
        public DateTime? BorrowerEmploymentHireDate { get; set; }
        public decimal? BorrowerIncome { get; set; }
        public string CoBorrowerName { get; set; }
        public DateTime? CoBorrowerBirthDate { get; set; }
        public Address CoBorrowerAddress { get; set; }
        public string CoBorrowerEmailAddress { get; set; }
        public string CoBorrowerPhone { get; set; }
        public string CoBorrowerEmployerName { get; set; }
        public DateTime? CoBorrowerEmploymentHireDate { get; set; }
        public decimal? CoBorrowerIncome { get; set; }
        public decimal? Amount { get; set; }
        public string Purpose { get; set; }
        public decimal? MortgagePayment { get; set; }
        public decimal? RentPayment { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
    }
}
