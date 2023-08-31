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
        private readonly List<ILoanApplicationProcessObserver> _observers = new List<ILoanApplicationProcessObserver>();

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
            LoanApplication createdApplication;
            ApiSettings apiSettings = await _settingsFactory.GetApiSettings();
            Queue<Task<LoanApplication>> createQueue = new Queue<Task<LoanApplication>>();
            foreach (LoanApplication loanApplication in _loanApplications)
            {
                while (createQueue.Count >= 3)
                {
                    createdApplication = await createQueue.Dequeue();
                    await NotifyObservers(_observers, this, createdApplication);
                }
                _logger.Information($"Creating loan application. Borrower {loanApplication.BorrowerName}");
                createQueue.Enqueue(_loanApplicationService.Create(apiSettings, loanApplication));
            }
            await NotifyObservers(
                _observers,
                this,
                await Task.WhenAll(createQueue));
        }

        private static Task NotifyObservers(IEnumerable<ILoanApplicationProcessObserver> observers, ILoanApplicationProcess process, params LoanApplication[] loanApplications)
        {
            List<Task> tasks = new List<Task>();
            foreach (ILoanApplicationProcessObserver observer in observers)
            {
                tasks.Add(observer.LoanApplicationCreated(process, loanApplications));
            }
            return Task.WhenAll(tasks);
        }

        public void AddObserver(ILoanApplicationProcessObserver observer)
            => _observers.Add(observer);
    }
}
