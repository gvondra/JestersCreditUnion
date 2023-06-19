using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanDataFactory
    {
        Task<LoanData> Get(ISqlSettings settings, Guid id);
        Task<LoanData> GetByNumber(ISqlSettings settings, string number);
        Task<LoanData> GetByLoanApplicationId(ISqlSettings settings, Guid loanApplicationId);
    }
}
