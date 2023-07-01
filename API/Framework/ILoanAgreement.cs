using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanAgreement
    {
        Guid LoanId { get; }
        LoanAgrementStatus Status { get; set; }
        DateTime CreateDate { get; }
        DateTime? AgreementDate { get; set; }
        string BorrowerName { get; set; }
        DateTime BorrowerBirthDate { get; set; }
        Guid? BorrowerAddressId { get; set; }
        Guid? BorrowerEmailAddressId { get; set; }
        Guid? BorrowerPhoneId { get; set; }
        string CoBorrowerName { get; set; }
        DateTime? CoBorrowerBirthDate { get; set; }
        Guid? CoBorrowerAddressId { get; set; }
        Guid? CoBorrowerEmailAddressId { get; set; }
        Guid? CoBorrowerPhoneId { get; set; }
        decimal OriginalAmount { get; set; }
        short OriginalTerm { get; set; }
        decimal InterestRate { get; set; }
        decimal PaymentAmount { get; set; }
        short PaymentFrequency { get; set; }

        Task Create(CommonCore.ITransactionHandler transactionHandler);
        Task Update(CommonCore.ITransactionHandler transactionHandler);
        Task<IAddress> GetBorrowerAddress(ISettings settings);
        Task<IAddress> GetCoBorrowerAddress(ISettings settings);
        Task<IEmailAddress> GetBorrowerEmailAddress(ISettings settings);
        Task<IEmailAddress> GetCoBorrowerEmailAddress(ISettings settings);
        Task<IPhone> GetBorrowerPhone(ISettings settings);
        Task<IPhone> GetCoBorrowerPhone(ISettings settings);
    }
}
