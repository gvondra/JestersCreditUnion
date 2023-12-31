using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanApplication
    {
        Guid LoanApplicationId { get; }
        Guid UserId { get; }
        LoanApplicationStatus Status { get; set; }
        DateTime ApplicationDate { get; }
        string BorrowerName { get; set; }
        DateTime BorrowerBirthDate { get; set; }
        //Guid? BorrowerAddressId { get; set; }
        string BorrowerEmailAddress { get; set; }
        string BorrowerPhone { get; set; }
        string BorrowerEmployerName { get; set; }
        DateTime? BorrowerEmploymentHireDate { get; set; }
        decimal? BorrowerIncome { get; set; }
        string CoBorrowerName { get; set; }
        DateTime? CoBorrowerBirthDate { get; set; }
        //Guid? CoBorrowerAddressId { get; set; }
        string CoBorrowerEmailAddress { get; set; }
        string CoBorrowerPhone { get; set; }
        string CoBorrowerEmployerName { get; set; }
        DateTime? CoBorrowerEmploymentHireDate { get; set; }
        decimal? CoBorrowerIncome { get; set; }
        decimal Amount { get; set; }
        string Purpose { get; set; }
        decimal? MortgagePayment { get; set; }
        decimal? RentPayment { get; set; }
        DateTime? ClosedDate { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }
        List<ILoanApplicationComment> Comments { get; }
        ILoanApplicationDenial Denial { get; }

        Task Create(ITransactionHandler transactionHandler, ISettings settings);
        Task Update(ITransactionHandler transactionHandler, ISettings settings);

        Task<IAddress> GetBorrowerAddress(ISettings settings);
        void SetBorrowerAddress(IAddress address);
        Task<IAddress> GetCoBorrowerAddress(ISettings settings);
        void SetCoBorrowerAddress(IAddress address);
        Task<string> GetStatusDescription(ISettings settings);
        ILoanApplicationComment CreateComment(string text, Guid userId, bool isInternal = true);
        void SetDenial(LoanApplicationDenialReason reason, DateTime date, Guid userId, string text);
        IIdentificationCardSaver CreateIdentificationCardSaver();
        IIdentificationCardReader CreateIdentificationCardReader();
    }
}
