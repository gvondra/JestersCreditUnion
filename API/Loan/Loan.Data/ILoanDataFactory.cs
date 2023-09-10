using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanDataFactory
    {
        Task<LoanData> Get(ISqlSettings settings, Guid id);
        Task<IEnumerable<LoanData>> GetByNameBirthDate(ISqlSettings settings, string name, DateTime birthDate);
        Task<IEnumerable<LoanData>> GetWithUnprocessedPayments(ISqlSettings settings);
        Task<LoanData> GetByNumber(ISqlSettings settings, string number);
        Task<LoanData> GetByLoanApplicationId(ISqlSettings settings, Guid loanApplicationId);
    }
}
