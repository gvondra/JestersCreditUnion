using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
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
        LoanStatus Status { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler);
        Task Update(ITransactionHandler transactionHandler);

        ITransaction CreateTransaction(DateTime date, TransactionType type, decimal amount);
        Task<IEnumerable<ITransaction>> GetTransactions(ISettings settings);
        Task<IEnumerable<IPayment>> GetPayments(ISettings settings);
    }
}
