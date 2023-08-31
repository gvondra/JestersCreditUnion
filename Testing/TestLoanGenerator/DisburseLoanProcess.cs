using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class DisburseLoanProcess : IDisburseLoanProcess, ILoanProcessObserver
    {
        private readonly IDisburseLoanProcess _process;
        private readonly ProcessQueue<Loan> _queue;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanService _loanService;
        private readonly ILogger _logger;
        private bool _disposedValue;

        public DisburseLoanProcess(
            ISettingsFactory settingsFactory,
            ILoanService loanService,
            ILogger logger)
        {
            _settingsFactory = settingsFactory;
            _loanService = loanService;
            _logger = logger;
            _process = this;
            _queue = new ProcessQueue<Loan>();
            _queue.ItemsDequeued += ItemsDequeued;
        }

        public void AddLoan(Loan loan)
        {
            _queue.Enqueue(loan);
        }

        public Task LoanCreated(ILoanProcess loanProcess, params Loan[] loans)
        {
            if (loans != null)
            {
                for (int i = 0; i < loans.Length; i += 1)
                {
                    _process.AddLoan(loans[i]);
                }
            }
            return Task.CompletedTask;
        }

        private void ItemsDequeued(object sender, IEnumerable<Loan> e)
        {
            if (e != null)
            {
                List<Task> tasks = new List<Task>();
                foreach (Loan loan in e)
                {
                    tasks.Add(
                        Disburse(loan));
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task Disburse(Loan loan)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
            await _loanService.DisburseFunds(settings, loan.LoanId.Value);
            _logger.Information($"Funds disbursed for loan number {loan.Number}");
        }

        public void WaitForProcessExit()
        {
            _queue.WaitForProcessExit();
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
