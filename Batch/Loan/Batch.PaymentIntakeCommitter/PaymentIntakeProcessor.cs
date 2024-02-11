using JestersCreditUnion.Interface.Loan;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.PaymentIntakeCommitter
{
    public class PaymentIntakeProcessor
    {
        private readonly ILogger<PaymentIntakeProcessor> _logger;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IPaymentIntakeService _paymentIntakeService;

        public PaymentIntakeProcessor(
            ILogger<PaymentIntakeProcessor> logger,
            ISettingsFactory settingsFactory,
            IPaymentIntakeService paymentIntakeService)
        {
            _logger = logger;
            _settingsFactory = settingsFactory;
            _paymentIntakeService = paymentIntakeService;
        }

        public async Task Process()
        {
            LoanSettings settings = _settingsFactory.CreateLoan();
            _logger.LogInformation("Committing payment intake items");
            await _paymentIntakeService.Commit(settings);
            _logger.LogInformation("Finished processing new payment intake items");
        }
    }
}
