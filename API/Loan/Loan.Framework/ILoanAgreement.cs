using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanAgreement
    {
        Guid LoanId { get; }
        LoanAgrementStatus Status { get; set; }
        DateTime CreateDate { get; }
        DateTime? AgreementDate { get; set; }
        string BorrowerName { get; set; }
        DateTime BorrowerBirthDate { get; set; }
        string BorrowerEmailAddress { get; set; }
        string BorrowerPhone { get; set; }
        string CoBorrowerName { get; set; }
        DateTime? CoBorrowerBirthDate { get; set; }
        string CoBorrowerEmailAddress { get; set; }
        string CoBorrowerPhone { get; set; }
        decimal OriginalAmount { get; set; }
        short OriginalTerm { get; set; }
        decimal InterestRate { get; set; }
        decimal PaymentAmount { get; set; }
        LoanPaymentFrequency PaymentFrequency { get; set; }

        Task Create(CommonCore.ITransactionHandler transactionHandler, ISettings settings);
        Task Update(CommonCore.ITransactionHandler transactionHandler, ISettings settings);
        Task<IAddress> GetBorrowerAddress(ISettings settings);
        void SetBorrowerAddress(IAddress address);
        Task<IAddress> GetCoBorrowerAddress(ISettings settings);
        void SetCoBorrowerAddress(IAddress address);
    }
}
