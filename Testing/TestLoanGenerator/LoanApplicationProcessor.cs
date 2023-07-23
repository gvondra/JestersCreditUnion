using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationProcessor : ILoanApplicationProcess
    {
        private readonly ILoanApplicationGenerator _loanApplicationGenerator;

        public LoanApplicationProcessor(ILoanApplicationGenerator loanApplicationGenerator)
        {
            _loanApplicationGenerator = loanApplicationGenerator;
        }

        public Task GenerateLoanApplications()
        {
            LoanApplication loanApplication = _loanApplicationGenerator.GenerateLoanApplication();
            return Task.CompletedTask;
        }
    }
}
