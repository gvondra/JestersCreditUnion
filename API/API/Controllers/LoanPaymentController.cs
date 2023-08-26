using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanPaymentController : APIControllerBase
    {
        private readonly ILoanFactory _loanFactory;
        private readonly IPaymentFactory _paymentFactory;
        private readonly IPaymentSaver _paymentSaver;

        public LoanPaymentController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanFactory loanFactory,
            IPaymentFactory paymentFactory,
            IPaymentSaver paymentSaver)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanFactory = loanFactory;
            _paymentFactory = paymentFactory;
            _paymentSaver = paymentSaver;
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [HttpPost("/api/Loan/Payment")]
        public async Task<IActionResult> Post(LoanPayment[] loanPayments)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (loanPayments == null)
                    loanPayments = Array.Empty<LoanPayment>();
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < loanPayments.Length; i += 1)
                {
                    tasks.Add(ValidateLoanPayment(loanPayments[i]));
                }
                await Task.WhenAll(tasks);
                if (loanPayments.Any(p => !string.IsNullOrEmpty(p.Message)))
                {
                    result = Ok(loanPayments);
                }
                else
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    List<IPayment> innerPayments = loanPayments.Select<LoanPayment, IPayment>(p => MapPayment(mapper, p))
                        .ToList();
                    innerPayments = (await _paymentSaver.Save(GetCoreSettings(), innerPayments)).ToList();
                    loanPayments = innerPayments.Select<IPayment, LoanPayment>(p => mapper.Map<LoanPayment>(p))
                        .ToArray();
                    for (int i = 0; i < loanPayments.Length; i += 1)
                    {
                        loanPayments[i].Message = "Saved. Pending Processing";
                    }
                    result = Ok(loanPayments);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("post-loan-payment", start, result);
            }
            return result;
        }

        private IPayment MapPayment(IMapper mapper, LoanPayment payment)
        {
            IPayment innerPayment = _paymentFactory.Create(payment.LoanNumber, payment.TransactionNumber, payment.Date.Value);
            mapper.Map(payment, innerPayment);
            return innerPayment;
        }

        private async Task ValidateLoanPayment(LoanPayment loanPayment)
        {
            StringBuilder message = new StringBuilder();
            if (string.IsNullOrEmpty(loanPayment.LoanNumber))
            {
                message.AppendLine("Missing loan number");
            }
            else
            {
                if ((await _loanFactory.GetByNumber(GetCoreSettings(), loanPayment.LoanNumber)) == null)
                    message.Append("Loan not found");
            }
            if (!loanPayment.Date.HasValue)
                message.AppendLine("Missing payment date");
            else if (DateTime.Today < loanPayment.Date.Value || loanPayment.Date.Value < DateTime.Today.AddYears(-100))
                message.AppendLine("Invalid payment date");
            if (!loanPayment.Amount.HasValue)
                message.AppendLine("Missing payment amount");
            loanPayment.Message = message.ToString();
        }
    }
}
