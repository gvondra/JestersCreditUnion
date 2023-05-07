using JestersCreditUnion.Interface.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanApplicationService
    {
        Task<LoanApplication> Get(ISettings settings, Guid id);
        Task<LoanApplication> Update(ISettings settings, LoanApplication loanApplication);
        Task<LoanApplicationComment> AppendComent(ISettings settings, Guid id, LoanApplicationComment comment, bool isPublic = false);
        Task Deny(ISettings settings, Guid id, LoanApplicationDenial denial);
    }
}
