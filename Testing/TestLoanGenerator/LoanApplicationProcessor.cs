using JestersCreditUnion.Interface.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationProcessor : ILoanApplicationProcess
    {
        private readonly IEnumerable<LoanApplication> _loanApplications;

        public LoanApplicationProcessor(
            Settings settings,
            Func<int, ILoanApplicationGenerator> createLoanApplicationGenerator,
            Func<string, IEnumerable<LoanApplication>, ILoanApplicationFileWriter> createLoanApplicationFileWriter)
        {
            _loanApplications = createLoanApplicationGenerator(settings.LoanApplicationCount);
            if (!string.IsNullOrEmpty(settings.LoanApplicationFile))
                _loanApplications = createLoanApplicationFileWriter(settings.LoanApplicationFile, _loanApplications);
        }

        public Task GenerateLoanApplications()
        {
            IEnumerator enumerator = _loanApplications.GetEnumerator();                
            while (enumerator.MoveNext()) { }
            return Task.CompletedTask;
        }
    }
}
