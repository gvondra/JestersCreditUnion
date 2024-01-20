using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IRatingFactory
    {
        IRating Create(double value, IEnumerable<IRatingLog> ratingLogs);
        IRatingLog CreateLog(double value, string description);
        Task<IRating> GetLoanApplication(ISettings settings, Guid loanApplicationId);
    }
}
