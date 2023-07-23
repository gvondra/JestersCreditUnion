using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationProcessor : ILoanApplicationProcess
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IEnumerable<LoanApplication> _loanApplications;
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanApplicationProcessor(
            ISettingsFactory settingsFactory,
            Settings settings,
            ILoanApplicationService loanApplicationService,
            Func<int, ILoanApplicationGenerator> createLoanApplicationGenerator,
            Func<string, IEnumerable<LoanApplication>, ILoanApplicationFileWriter> createLoanApplicationFileWriter)
        {
            _settingsFactory = settingsFactory;
            _loanApplications = createLoanApplicationGenerator(settings.LoanApplicationCount);
            _loanApplicationService = loanApplicationService;
            if (!string.IsNullOrEmpty(settings.LoanApplicationFile))
                _loanApplications = createLoanApplicationFileWriter(settings.LoanApplicationFile, _loanApplications);
        }

        public async Task GenerateLoanApplications()
        {
            ApiSettings apiSettings = await _settingsFactory.GetApiSettings();
            foreach (LoanApplication loanApplication in _loanApplications)
            {
                await _loanApplicationService.Create(apiSettings, loanApplication);
            }
        }
    }
}
