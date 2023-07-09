using JestersCreditUnion.CommonCore;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoan
    {
        Guid LoanId { get; }
        string Number { get; }
        Guid LoanApplicationId { get; }
        ILoanAgreement Agreement { get; }
        DateTime? InitialDisbursementDate { get; set; }
        DateTime? FirstPaymentDue { get; set; }
        DateTime? NextPaymentDue { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler);
        Task Update(ITransactionHandler transactionHandler);
    }
}
