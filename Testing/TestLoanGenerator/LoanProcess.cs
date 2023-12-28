using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanProcess : ILoanProcess, ILoanApplicationProcessObserver
    {
        private readonly ILoanProcess _process;
        private readonly ProcessQueue<LoanApplication> _queue;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanService _loanService;
        private readonly ILoanPaymentAmountService _loanPaymentAmountService;
        private readonly ILogger _logger;
        private readonly List<ILoanProcessObserver> _observers = new List<ILoanProcessObserver>();
        private bool _disposedValue;
        private int _count;

        public LoanProcess(
            ISettingsFactory settingsFactory,
            ILoanService loanService,
            ILoanPaymentAmountService loanPaymentAmountService,
            ILogger logger)
        {
            _settingsFactory = settingsFactory;
            _loanService = loanService;
            _loanPaymentAmountService = loanPaymentAmountService;
            _logger = logger;
            _process = this;
            _queue = new ProcessQueue<LoanApplication>();
            _queue.ItemsDequeued += LoanApplicationDequeued;
        }

        private void LoanApplicationDequeued(object sender, IEnumerable<LoanApplication> e)
        {
            if (e != null)
            {
                foreach (LoanApplication application in e)
                {
                    _count += 1;
                    Loan loan = CreateLoan(application).Result;
                    _logger.Information($"Loan #{_count:###,###,##0} created with number {loan.Number}");
                    InitiateDisbursement(loan).Wait();
                    NotifiyObservers(loan).Wait();
                }
            }
        }

        private Task NotifiyObservers(Loan loan)
        {
            List<Task> tasks = new List<Task>();
            foreach (ILoanProcessObserver observer in _observers)
            {
                tasks.Add(observer.LoanCreated(_process, loan));
            }
            return Task.WhenAll(tasks);
        }

        private async Task InitiateDisbursement(Loan loan)
        {
            LoanApiSettings settings = await _settingsFactory.GetLoanApiSettings();
            await _loanService.InitiateDisbursement(settings, loan.LoanId.Value);
        }

        private async Task<Loan> CreateLoan(LoanApplication loanApplication)
        {
            LoanAgreement agreement = new LoanAgreement
            {
                BorrowerAddress = loanApplication.BorrowerAddress,
                BorrowerBirthDate = loanApplication.BorrowerBirthDate,
                BorrowerEmailAddress = loanApplication.BorrowerEmailAddress,
                BorrowerName = loanApplication.BorrowerName,
                BorrowerPhone = loanApplication.BorrowerPhone,
                OriginalAmount = loanApplication.Amount,
                CreateDate = DateTime.Today,
                InterestRate = 0.02M,
                PaymentFrequency = 12,
                OriginalTerm = 48
            };
            Loan loan = new Loan
            {
                Agreement = agreement,
                LoanApplicationId = loanApplication.LoanApplicationId.Value                
            };
            LoanApiSettings loanApiSettings = await _settingsFactory.GetLoanApiSettings();
            loan.Agreement.PaymentAmount = await GetPaymentAmount(loanApiSettings, loan);
            return await _loanService.Create(loanApiSettings, loan);
        }

        private async Task<decimal> GetPaymentAmount(LoanApiSettings settings, Loan loan)
        {
            LoanPaymentAmountRequest requet = new LoanPaymentAmountRequest
            {
                AnnualInterestRate = loan.Agreement.InterestRate,
                PaymentFrequency = loan.Agreement.PaymentFrequency.Value,
                Term = loan.Agreement.OriginalTerm.Value,
                TotalPrincipal = loan.Agreement.OriginalAmount.Value
            };
            LoanPaymentAmountResponse response = await _loanPaymentAmountService.Calculate(settings, requet);
            return response.PaymentAmount.Value;
        }

        public void AddLoanApplication(LoanApplication application)
        {
            _queue.Enqueue(application);
        }

        public Task LoanApplicationCreated(ILoanApplicationProcess loanApplicationProcess, params LoanApplication[] loanApplications)
        {
            if (loanApplications != null)
            {
                for (int i = 0; i < loanApplications.Length; i += 1)
                {
                    _process.AddLoanApplication(loanApplications[i]);
                }
            }
            return Task.CompletedTask;
        }

        public void AddObserver(ILoanProcessObserver observer)
        {
            _observers.Add(observer);
        }

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
