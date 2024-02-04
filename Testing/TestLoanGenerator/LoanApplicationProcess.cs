using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationProcess : ILoanApplicationProcess
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IEnumerable<LoanApplication> _loanApplications;
        private readonly ILoanApplicationService _loanApplicationService;
        private readonly ILogger _logger;
        private readonly List<ILoanApplicationProcessObserver> _observers = new List<ILoanApplicationProcessObserver>();

        public LoanApplicationProcess(
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
            LoanApplication createdApplication;
            LoanApiSettings apiSettings = await _settingsFactory.GetLoanApiSettings();
            Queue<Task<LoanApplication>> createQueue = new Queue<Task<LoanApplication>>();
            int i = 1;
            foreach (LoanApplication loanApplication in _loanApplications)
            {
                while (createQueue.Count >= 4)
                {
                    createdApplication = await createQueue.Dequeue();
                    await NotifyObservers(_observers, createdApplication);
                }
                _logger.Information($"Creating loan application #{i:###,###,##0}. Borrower {loanApplication.BorrowerName}");
                createQueue.Enqueue(Create(apiSettings, loanApplication));
                i += 1;
            }
            await NotifyObservers(
                _observers,
                await Task.WhenAll(createQueue));
        }

        private async Task<LoanApplication> Create(LoanApiSettings apiSettings, LoanApplication loanApplication)
        {
            loanApplication = await _loanApplicationService.Create(apiSettings, loanApplication);
            loanApplication.Status = 1;
            loanApplication = await _loanApplicationService.Update(apiSettings, loanApplication);
            return loanApplication;
        }

        private static Task NotifyObservers(IEnumerable<ILoanApplicationProcessObserver> observers,params LoanApplication[] loanApplications)
        {
            List<Task> tasks = new List<Task>();
            foreach (ILoanApplicationProcessObserver observer in observers)
            {
                tasks.Add(observer.LoanApplicationCreated(loanApplications));
            }
            return Task.WhenAll(tasks);
        }

        public void AddObserver(ILoanApplicationProcessObserver observer)
            => _observers.Add(observer);
    }
}
