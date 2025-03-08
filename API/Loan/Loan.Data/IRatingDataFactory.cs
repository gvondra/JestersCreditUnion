using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IRatingDataFactory
    {
        Task<RatingData> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
    }
}
