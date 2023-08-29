using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class LoanPaymentProcessor
    {
        private const int _threadCount = 4;
        private const int _paymentsPerThread = 2;
        private readonly ILogger<LoanPaymentProcessor> _logger;
        private readonly SettingsFactory _settingsFactory;
        private readonly IPaymentFactory _paymentFactory;
        private readonly ILoanPaymentProcessor _paymentProcessor;
        private ConcurrentQueue<IPayment> _paymentQueue;

        public LoanPaymentProcessor(
            ILogger<LoanPaymentProcessor> logger,
            SettingsFactory settingsFactory,
            IPaymentFactory paymentFactory,
            ILoanPaymentProcessor paymentProcessor)
        {
            _logger = logger;
            _settingsFactory = settingsFactory;
            _paymentFactory = paymentFactory;
            _paymentProcessor = paymentProcessor;
        }

        public async Task Process()
        {
            _paymentQueue = new ConcurrentQueue<IPayment>(
                (await _paymentFactory.GetByStatus(_settingsFactory.CreateCore(), PaymentStatus.Unprocessed))
                .Where(p => p.Date <= DateTime.Today && p.Amount > 0.0M));
            _logger.LogInformation($"Loaded {_paymentQueue.Count:###,###,##1} payments");
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
            IPayment payment;
            while (_paymentQueue.TryDequeue(out payment))
            {
                while (tasks.Count > _paymentsPerThread)
                {
                    await tasks.Dequeue();
                }
                tasks.Enqueue(ProcessPayment(settings, payment));
            }
            await Task.WhenAll(tasks);
        }

        private async Task ProcessPayment(CoreSettings settings, IPayment payment)
        {
            _logger.LogInformation($"Start processing payment with loan number = {payment.LoanId:D}, transaction number = {payment.TransactionNumber}, date = {payment.Date:yyyy-MM-dd}, and amount = {payment.Amount:$###,###,##0.00}");
            await _paymentProcessor.Process(settings, payment);
        }
    }
}
