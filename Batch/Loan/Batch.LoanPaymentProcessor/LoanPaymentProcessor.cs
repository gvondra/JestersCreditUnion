using JestersCreditUnion.Loan.Framework;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class LoanPaymentProcessor
    {
        private const int _threadCount = 4;
        private const int _loansPerThread = 2;
        private readonly ILogger<LoanPaymentProcessor> _logger;
        private readonly SettingsFactory _settingsFactory;
        private readonly ILoanFactory _loanFactory;
        private readonly ILoanPaymentProcessor _paymentProcessor;
        private ConcurrentQueue<ILoan> _loanQueue;

        public LoanPaymentProcessor(
            ILogger<LoanPaymentProcessor> logger,
            SettingsFactory settingsFactory,
            ILoanFactory loanFactory,
            ILoanPaymentProcessor paymentProcessor)
        {
            _logger = logger;
            _settingsFactory = settingsFactory;
            _loanFactory = loanFactory;
            _paymentProcessor = paymentProcessor;
        }

        public async Task Process()
        {
            _loanQueue = new ConcurrentQueue<ILoan>(
                (await _loanFactory.GetWithUnprocessedPayments(_settingsFactory.CreateCore())));
            _logger.LogInformation($"Loaded {_loanQueue.Count:###,###,##0} loans");
            Task[] tasks = new Task[_threadCount - 1];
            for (int i = 0; i < tasks.Length; i += 1)
            {
                tasks[i] = Task.Run(ProcessPaymentQueue);
            }
            await ProcessPaymentQueue();
            await Task.WhenAll(tasks);
        }

        private async Task ProcessPaymentQueue()
        {
            CoreSettings settings = _settingsFactory.CreateCore();
            Queue<Task> tasks = new Queue<Task>();
            ILoan loan;
            while (_loanQueue.TryDequeue(out loan))
            {
                while (tasks.Count > _loansPerThread)
                {
                    await tasks.Dequeue();
                }
                tasks.Enqueue(ProcessPayment(settings, loan));
            }
            await Task.WhenAll(tasks);
        }

        private async Task ProcessPayment(CoreSettings settings, ILoan loan)
        {
            _logger.LogInformation($"Start processing payments for loan number = {loan.Number}");
            await _paymentProcessor.Process(settings, loan);
        }
    }
}
