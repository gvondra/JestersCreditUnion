using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationProcessor : ILoanApplicationProcess
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IEnumerable<LoanApplication> _loanApplications;
        private readonly ILoanApplicationService _loanApplicationService;
        private readonly ILogger _logger;

        public LoanApplicationProcessor(
            ISettingsFactory settingsFactory,
            Settings settings,
            ILoanApplicationService loanApplicationService,
            ILogger logger,
            Func<int, ILoanApplicationGenerator> createLoanApplicationGenerator,
            Func<string, IEnumerable<LoanApplication>, ILoanApplicationFileWriter> createLoanApplicationFileWriter)
        {
            _settingsFactory = settingsFactory;
            _loanApplications = createLoanApplicationGenerator(settings.LoanApplicationCount);
            _loanApplicationService = loanApplicationService;
            _logger = logger;
            if (!string.IsNullOrEmpty(settings.LoanApplicationFile))
                _loanApplications = createLoanApplicationFileWriter(settings.LoanApplicationFile, _loanApplications);
        }

        public async Task GenerateLoanApplications()
        {
            ApiSettings apiSettings = await _settingsFactory.GetApiSettings();
            Queue<Task> createQueue = new Queue<Task>();
            foreach (LoanApplication loanApplication in _loanApplications)
            {
                while (createQueue.Count >= 3)
                {
                    await createQueue.Dequeue();
                }
                _logger.Information($"Creating loan. Borrower {loanApplication.BorrowerName}");
                createQueue.Enqueue(_loanApplicationService.Create(apiSettings, loanApplication));
            }
            await Task.WhenAll(createQueue);
        }
    }
}
