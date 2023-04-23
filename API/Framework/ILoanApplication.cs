using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplication
    {
        Guid LoanApplicationId { get; }
        Guid UserId { get; }
        LoanApplicationStatus Status { get; set; }
        DateTime ApplicationDate { get; }
        string BorrowerName { get; set; }
        DateTime BorrowerBirthDate { get; set; }
        Guid? BorrowerAddressId { get; set; }
        Guid? BorrowerEmailAddressId { get; set; }
        Guid? BorrowerPhoneId { get; set; }
        string BorrowerEmployerName { get; set; }
        DateTime? BorrowerEmploymentHireDate { get; set; }
        decimal? BorrowerIncome { get; set; }
        string CoBorrowerName { get; set; }
        DateTime? CoBorrowerBirthDate { get; set; }
        Guid? CoBorrowerAddressId { get; set; }
        Guid? CoBorrowerEmailAddressId { get; set; }
        Guid? CoBorrowerPhoneId { get; set; }
        string CoBorrowerEmployerName { get; set; }
        DateTime? CoBorrowerEmploymentHireDate { get; set; }
        decimal? CoBorrowerIncome { get; set; }
        decimal Amount { get; set; }
        string Purpose { get; set; }
        decimal? MortgagePayment { get; set; }
        decimal? RentPayment { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }

        Task Create(ISettings settings);
        Task Update(ISettings settings);

        Task<IAddress> GetBorrowerAddress(ISettings settings);
        Task<IAddress> GetCoBorrowerAddress(ISettings settings);
        Task<IEmailAddress> GetBorrowerEmailAddress(ISettings settings);
        Task<IEmailAddress> GetCoBorrowerEmailAddress(ISettings settings);
        Task<IPhone> GetBorrowerPhone(ISettings settings);
        Task<IPhone> GetCoBorrowerPhone(ISettings settings);
        Task<string> GetStatusDescription(ISettings settings);
    }
}
