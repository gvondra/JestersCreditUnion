using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPayment
    {
        Guid PaymentId { get; }
        Guid LoanId { get; }
        string TransactionNumber { get; }
        DateTime Date { get; }
        decimal Amount { get; set; }
        PaymentStatus Status { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }
        List<IPaymentTransaction> Transactions { get; }

        Task Update(ITransactionHandler transactionHandler);
        Task<ILoan> GetLoan(ISettings settings);
        IPaymentTransaction CreatePaymentTransaction(ILoan loan, DateTime date, TransactionType type, decimal amount);
    }
}
