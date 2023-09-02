using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using JestersCreditUnion.Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanPaymentAmountController : APIControllerBase
    {
        private readonly ILoanPaymentAmountCalculator _loanPaymentAmountCalculator;

        public LoanPaymentAmountController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanPaymentAmountCalculator loanPaymentAmountCalculator)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanPaymentAmountCalculator = loanPaymentAmountCalculator;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(LoanPaymentAmountResponse), 200)]
        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] LoanPaymentAmountRequest request)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                result = Validate(request);
                if (result == null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    LoanPaymentAmountResponse loanPaymentAmountResponse = mapper.Map<LoanPaymentAmountResponse>(request);
                    loanPaymentAmountResponse.PaymentAmount = _loanPaymentAmountCalculator.Calculate(
                        request.TotalPrincipal.Value,
                        request.AnnualInterestRate.Value,
                        request.Term.Value,
                        (LoanPaymentFrequency)request.PaymentFrequency);
                    result = Ok(loanPaymentAmountResponse);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("calculate-loan-payment-amount", start, result);
            }
            return result;
        }

        [NonAction]
        private IActionResult Validate(LoanPaymentAmountRequest request)
        {
            IActionResult result = null;
            if (request == null)
                result = BadRequest("Missing request body");
            else if (!request.TotalPrincipal.HasValue)
                result = BadRequest("Missing total principal amount");
            else if (request.TotalPrincipal.Value <= 0.0M)
                result = BadRequest("Total principal must be greater than zero");
            else if (!request.AnnualInterestRate.HasValue)
                result = BadRequest("Missing annual interest rate");
            else if (request.AnnualInterestRate.Value <= 0.0M || request.AnnualInterestRate >= 1.0M)
                result = BadRequest("Annual interest rate must be between 0.0 and 1.0, exclusive");
            else if (!request.Term.HasValue)
                result = BadRequest("Missing term value");
            else if (request.Term.Value <= 0.0M)
                result = BadRequest("Term must be greater than zero");
            return result;
        }
    }
}
