using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanDataFactory
    {
        Task<LoanData> GetByNumber(IDataSettings settings, string number);
        Task<LoanData> GetByLoanApplicationId(IDataSettings settings, Guid loanApplicationId);
    }
}
