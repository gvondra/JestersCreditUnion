using JestersCreditUnion.Interface.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanService
    {
        Task<Loan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
        Task<Loan> Create(ISettings settings, Loan loan);
        Task<Loan> Update(ISettings settings, Loan loan);
    }
}
