using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationProcessObserver
    {
        Task LoanApplicationCreated(params LoanApplication[] loanApplications);
        Task NewLoanApplicationTaskCreated(params LoanApplication[] loanApplications);
        Task LoanApplicationApproved(params LoanApplication[] loanApplications);
        Task LoanApplicationDenied(params LoanApplication[] loanApplications);
    }
}
