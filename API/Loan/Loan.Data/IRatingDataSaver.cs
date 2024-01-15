using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IRatingDataSaver
    {
        Task SaveLoanApplicationRating(ISqlTransactionHandler transactionHandler, Guid loanApplicationId, RatingData data);
    }
}
