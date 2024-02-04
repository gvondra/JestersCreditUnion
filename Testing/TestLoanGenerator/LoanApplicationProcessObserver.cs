using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public abstract class LoanApplicationProcessObserver : ILoanApplicationProcessObserver
    {
        public virtual Task LoanApplicationApproved(params LoanApplication[] loanApplications)
        {
            // do nothing
            return Task.CompletedTask;
        }

        public virtual Task LoanApplicationCreated(params LoanApplication[] loanApplications)
        {
            // do nothing
            return Task.CompletedTask;
        }

        public virtual Task LoanApplicationDenied(params LoanApplication[] loanApplications)
        {
            // do nothing
            return Task.CompletedTask;
        }

        public virtual Task NewLoanApplicationTaskCreated(params LoanApplication[] loanApplications)
        {
            // do nothing
            return Task.CompletedTask;
        }
    }
}
