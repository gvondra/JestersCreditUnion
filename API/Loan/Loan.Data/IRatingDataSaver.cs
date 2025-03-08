using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IRatingDataSaver
    {
        Task SaveLoanApplicationRating(ITransactionHandler transactionHandler, Guid loanApplicationId, RatingData data);
    }
}
