using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.PaymentIntakeCommitter
{
    public class PaymentIntakeProcessor
    {
        private readonly ILogger<PaymentIntakeProcessor> _logger;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IPaymentIntakeService _paymentIntakeService;
        private readonly ILoanPaymentService _loanPaymentService;

        public PaymentIntakeProcessor(
            ILogger<PaymentIntakeProcessor> logger,
            ISettingsFactory settingsFactory,
            IPaymentIntakeService paymentIntakeService,
            ILoanPaymentService loanPaymentService)
        {
            _logger = logger;
            _settingsFactory = settingsFactory;
            _paymentIntakeService = paymentIntakeService;
            _loanPaymentService = loanPaymentService;
        }

        public async Task Process()
        {
            LoanSettings settings = _settingsFactory.CreateLoan();
            _logger.LogInformation("Fetching new payment intake items");
            List<PaymentIntake> paymentIntakes = await _paymentIntakeService.GetByStatuses(settings, new short[] { 0 });
            _logger.LogInformation($"Found {paymentIntakes.Count} items");
            List<LoanPayment> loanPayments = new List<LoanPayment>();
            foreach (PaymentIntake paymentIntake in paymentIntakes)
            {
                LoanPayment loanPayment = new LoanPayment
                {
                    Amount = paymentIntake.Amount,
                    Date = paymentIntake.Date,
                    LoanNumber = paymentIntake.Loan.Number,
                    TransactionNumber = paymentIntake.TransactionNumber
                };
            }
            //loanPayments = await _loanPaymentService.Save(settings, loanPayments);
            _logger.LogInformation("Finished processing new payment intake items");
        }
    }
}
