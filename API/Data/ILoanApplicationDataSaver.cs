using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanApplicationDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, LoanApplicationData data);
        Task Update(ISqlTransactionHandler transactionHandler, LoanApplicationData data);
        Task AppendComment(ISqlTransactionHandler transactionHandler, LoanApplicationCommentData data);
        Task SetDenial(ISqlTransactionHandler transactionHandler, Guid id, short loanApplicationStatus, LoanApplicationDenialData denial);
    }
}
