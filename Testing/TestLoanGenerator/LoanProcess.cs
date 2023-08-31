using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
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
        private bool _disposedValue;

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
                List<Task> tasks = new List<Task>();
                foreach (LoanApplication application in e)
                {
                    Loan loan = CreateLoan(application).Result;
                    _logger.Information($"Created loan number {loan.Number}");
                    tasks.Add(
                        InitiateDisbursement(loan));
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task InitiateDisbursement(Loan loan)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
            await _loanService.InitiateDisbursement(settings, loan.LoanId.Value);
        }

        private async Task<Loan> CreateLoan(LoanApplication loanApplication)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
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
            loan.Agreement.PaymentAmount = await GetPaymentAmount(settings, loan);
            return await _loanService.Create(settings, loan);
        }

        private async Task<decimal> GetPaymentAmount(ApiSettings settings, Loan loan)
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
    }
}
