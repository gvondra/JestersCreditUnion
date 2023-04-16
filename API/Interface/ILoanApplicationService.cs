using JestersCreditUnion.Interface.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanApplicationService
    {
        Task<LoanApplication> Get(ISettings settings, Guid id);
    }
}
