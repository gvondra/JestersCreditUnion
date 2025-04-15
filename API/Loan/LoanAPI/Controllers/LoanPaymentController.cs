using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Loan.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanPaymentController : LoanApiControllerBase
    {
        private readonly ILoanFactory _loanFactory;
        private readonly IPaymentFactory _paymentFactory;
        private readonly IPaymentSaver _paymentSaver;

        public LoanPaymentController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanPaymentController> logger,
            ILoanFactory loanFactory,
            IPaymentFactory paymentFactory,
            IPaymentSaver paymentSaver)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanFactory = loanFactory;
            _paymentFactory = paymentFactory;
            _paymentSaver = paymentSaver;
        }

        private static void ValidateLoanPayment(ILoan loan, LoanPayment loanPayment)
        {
            StringBuilder message = new StringBuilder();
            if (string.IsNullOrEmpty(loanPayment.LoanNumber))
            {
                message.AppendLine("Missing loan number");
            }
            else if (loan == null)
            {
                message.AppendLine("Loan not found");
            }
            if (!loanPayment.Date.HasValue)
                message.AppendLine("Missing payment date");
            else if (DateTime.Today < loanPayment.Date.Value || loanPayment.Date.Value < DateTime.Today.AddYears(-100))
                message.AppendLine("Invalid payment date");
            if (!loanPayment.Amount.HasValue)
                message.AppendLine("Missing payment amount");
            loanPayment.Message = message.ToString();
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [HttpPost("/api/Loan/Payment")]
        public async Task<IActionResult> Post([FromBody] LoanPayment[] loanPayments)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                Dictionary<string, ILoan> loans = new Dictionary<string, ILoan>();
                CoreSettings settings = GetCoreSettings();
                if (loanPayments == null)
                    loanPayments = Array.Empty<LoanPayment>();
                for (int i = 0; i < loanPayments.Length; i += 1)
                {
                    ILoan loan = null;
                    if (!string.IsNullOrEmpty(loanPayments[i].LoanNumber))
                    {
                        loan = await _loanFactory.GetByNumber(settings, loanPayments[i].LoanNumber);
                        if (loan != null)
                            loans[loanPayments[i].LoanNumber] = loan;
                    }
                    ValidateLoanPayment(loan, loanPayments[i]);
                }
                if (Array.Exists(loanPayments, p => !string.IsNullOrEmpty(p.Message)))
                {
                    result = Ok(loanPayments);
                }
                else
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    List<IPayment> innerPayments = loanPayments.Select<LoanPayment, IPayment>(p => MapPayment(mapper, loans[p.LoanNumber], p))
                        .ToList();
                    innerPayments = (await _paymentSaver.Save(settings, innerPayments)).ToList();
                    loanPayments = innerPayments.Select<IPayment, LoanPayment>(mapper.Map<LoanPayment>)
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

        private IPayment MapPayment(IMapper mapper, ILoan loan, LoanPayment payment)
        {
            IPayment innerPayment = _paymentFactory.Create(loan, payment.TransactionNumber, payment.Date.Value);
            mapper.Map(payment, innerPayment);
            return innerPayment;
        }
    }
}
