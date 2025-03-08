using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface ILoanDataFactory
    {
        Task<LoanData> Get(ISettings settings, Guid id);
        Task<IEnumerable<LoanData>> GetByNameBirthDate(ISettings settings, string name, DateTime birthDate);
        Task<IEnumerable<LoanData>> GetWithUnprocessedPayments(ISettings settings);
        Task<LoanData> GetByNumber(ISettings settings, string number);
        Task<LoanData> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
    }
}
