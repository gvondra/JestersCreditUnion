using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using Serilog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class NewLoanApplicationTaskProcess : LoanApplicationProcessObserver, INewLoanApplicationTaskProcess
    {
        private readonly ProcessQueue<LoanApplication> _queue;
        private readonly ILogger _logger;
        private readonly List<ILoanApplicationProcessObserver> _observers = new List<ILoanApplicationProcessObserver>();
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanApplicationService _loanApplicationService;
        private readonly ILoanApplicationRatingService _ratingService;
        private bool _disposedValue;
        private int _count;

        public NewLoanApplicationTaskProcess(
            ISettingsFactory settingsFactory,
            ILoanApplicationService loanApplicationService,
            ILoanApplicationRatingService ratingService,
            ILogger logger)
        {
            _queue = new ProcessQueue<LoanApplication>();
            _queue.ItemsDequeued += LoanApplicationDequeued;
            _settingsFactory = settingsFactory;
            _loanApplicationService = loanApplicationService;
            _ratingService = ratingService;
            _logger = logger;

        }

        public int Count => _count;

        private void LoanApplicationDequeued(object sender, IEnumerable<LoanApplication> e)
        {
            if (e != null)
            {
                List<Task> tasks = new List<Task>();
                foreach (LoanApplication application in e)
                {
                    tasks.Add(
                        ProcessApplication(application));
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task ProcessApplication(LoanApplication application)
        {
            bool approve = false;
            if ((application.Amount ?? 0.0M) <= 100000M)
            {
                LoanApiSettings settings = await _settingsFactory.GetLoanApiSettings();
                Rating rating = await _ratingService.Get(settings, application.LoanApplicationId.Value);
                approve = (rating.Value ?? 0.0) >= 0.2;
            }
            if (approve)
                await Aprove(application);
            else
                await Deny(application);
            Interlocked.Increment(ref _count);
        }

        private async Task Aprove(LoanApplication application)
        {
            _logger.Information($"Approving {application.Amount:$###,###,##0.00} for {application.BorrowerName} born {application.CoBorrowerBirthDate:yyyy-MM-dd}");
            LoanApiSettings settings = await _settingsFactory.GetLoanApiSettings();
            application.Status = 3;
            application = await _loanApplicationService.Update(settings, application);
            NotifyApproved(application);
        }

        private async Task Deny(LoanApplication application)
        {
            _logger.Information($"Denying {application.Amount:$###,###,##0.00} for {application.BorrowerName} born {application.CoBorrowerBirthDate:yyyy-MM-dd}");
            LoanApiSettings settings = await _settingsFactory.GetLoanApiSettings();
            LoanApplicationDenial denial = new LoanApplicationDenial
            {
                Date = DateTime.Today,
                Reason = 0,
                Text = "Automated Test"
            };
            await _loanApplicationService.Deny(settings, application.LoanApplicationId.Value, denial);
            NotifyDenied(application);
        }

        private void NotifyDenied(LoanApplication application)
        {
            foreach (ILoanApplicationProcessObserver observer in _observers)
            {
                observer.LoanApplicationDenied(application);
            }
        }

        private void NotifyApproved(LoanApplication application)
        {
            foreach (ILoanApplicationProcessObserver observer in _observers)
            {
                observer.LoanApplicationApproved(application);
            }
        }

        public void AddLoanApplication(LoanApplication application)
        {
            _queue.Enqueue(application);
        }

        public override Task NewLoanApplicationTaskCreated(params LoanApplication[] loanApplications)
        {
            if (loanApplications != null)
            {
                for (int i = 0; i < loanApplications.Length; i += 1)
                {
                    AddLoanApplication(loanApplications[i]);
                }
            }
            return Task.CompletedTask;
        }

        public void AddObserver(ILoanApplicationProcessObserver observer)
            => _observers.Add(observer);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _queue.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void WaitForProcessExit()
        {
            _queue.WaitForProcessExit();
        }
    }
}
