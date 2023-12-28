using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationProcessObserver
    {
        Task LoanApplicationCreated(ILoanApplicationProcess loanApplicationProcess, params LoanApplication[] loanApplications);
    }
}
