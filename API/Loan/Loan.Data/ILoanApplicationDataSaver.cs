using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface ILoanApplicationDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, LoanApplicationData data);
        Task Update(ITransactionHandler transactionHandler, LoanApplicationData data);
        Task AppendComment(ITransactionHandler transactionHandler, LoanApplicationCommentData data);
        Task SetDenial(ITransactionHandler transactionHandler, Guid id, short loanApplicationStatus, DateTime? closedDate, LoanApplicationDenialData denial);
    }
}
