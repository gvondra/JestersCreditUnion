using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IRatingSaver
    {
        Task SaveLoanApplicationRating(ISettings settings, Guid loanApplicationId, IRating rating);
    }
}
