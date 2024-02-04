using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanApplicationRatingService
    {
        Task<Rating> Get(ISettings settings, Guid loanApplicationId);
        Task<Rating> Create(ISettings settings, Guid loanApplicationId);
    }
}
