using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentIntakeController : LoanApiControllerBase
    {
        private readonly IPaymentIntakeFactory _paymentIntakeFactory;
        private readonly IPaymentIntakeSaver _paymentIntakeSaver;
        private readonly ILoanFactory _loanFactory;
        private readonly AuthorizationAPI.IUserService _userService;

        public PaymentIntakeController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<PaymentIntakeController> logger,
            IPaymentIntakeFactory paymentIntakeFactory,
            IPaymentIntakeSaver paymentIntakeSaver,
            ILoanFactory loanFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _paymentIntakeFactory = paymentIntakeFactory;
            _paymentIntakeSaver = paymentIntakeSaver;
            _loanFactory = loanFactory;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_LOAN_EDIT)]
        [ProducesResponseType(typeof(PaymentIntake[]), 200)]
        public async Task<IActionResult> Search([FromQuery] short[] status = null)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                List<PaymentIntake> paymentIntakes = null;
                if (status != null && status.Length > 0)
                    paymentIntakes = await GetByStatues(status);
                result = Ok(paymentIntakes);
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("search-payment-intake", start, result);
            }
            return result;
        }

        [NonAction]
        private async Task<List<PaymentIntake>> GetByStatues(short[] statuses)
        {
            IMapper mapper = MapperConfiguration.CreateMapper();
            CoreSettings settings = GetCoreSettings();
            return (await Task.WhenAll((await _paymentIntakeFactory.GetByStatues(settings, statuses.Select(s => (PaymentIntakeStatus)s)))
                .Select(pi => Map(settings, mapper, pi))))
                .ToList();
        }

        [NonAction]
        private IActionResult Validate(PaymentIntake paymentIntake)
        {
            IActionResult result = null;
            if (paymentIntake == null)
                result = BadRequest("Missing request body");
            else if (!paymentIntake.Date.HasValue)
                result = BadRequest("Missing date value");
            else if (!paymentIntake.Amount.HasValue)
                result = BadRequest("Missing amount value");
            return result;
        }

        [HttpPost("/api/Loan/{loanId}/PaymentIntake")]
        [Authorize(Constants.POLICY_LOAN_EDIT)]
        [ProducesResponseType(typeof(PaymentIntake), 200)]
        public async Task<IActionResult> Create([FromRoute] Guid loanId, [FromBody] PaymentIntake paymentIntake)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                result = Validate(paymentIntake);
                CoreSettings settings = GetCoreSettings();
                ILoan loan = null;
                if (result == null)
                {
                    loan = await _loanFactory.Get(settings, loanId);
                    if (loan == null)
                        result = NotFound();
                }
                if (result == null && loan != null)
                {
                    IPaymentIntake innerPaymentIntake = _paymentIntakeFactory.Create(loan);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    mapper.Map(paymentIntake, innerPaymentIntake);
                    Guid? userId = await GetCurrentUserId();
                    await _paymentIntakeSaver.Create(settings, innerPaymentIntake, userId.HasValue ? userId.Value.ToString("D") : string.Empty);
                    result = Ok(await Map(settings, mapper, innerPaymentIntake));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("create-payment-intake", start, result);
            }
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Constants.POLICY_LOAN_EDIT)]
        [ProducesResponseType(typeof(PaymentIntake), 200)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PaymentIntake paymentIntake)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                result = Validate(paymentIntake);
                CoreSettings settings = GetCoreSettings();
                IPaymentIntake innerPaymentIntake = null;
                if (result == null)
                {
                    innerPaymentIntake = await _paymentIntakeFactory.Get(settings, id);
                    if (innerPaymentIntake == null)
                        result = NotFound();
                }
                if (result == null && innerPaymentIntake != null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    mapper.Map(paymentIntake, innerPaymentIntake);
                    Guid? userId = await GetCurrentUserId();
                    await _paymentIntakeSaver.Update(settings, innerPaymentIntake, userId.HasValue ? userId.Value.ToString("D") : string.Empty);
                    result = Ok(await Map(settings, mapper, innerPaymentIntake));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("update-payment-intake", start, result);
            }
            return result;
        }

        [NonAction]
        private async Task<PaymentIntake> Map(
            CoreSettings settings,
            IMapper mapper,
            IPaymentIntake paymentIntake)
        {
            PaymentIntake result = mapper.Map<PaymentIntake>(paymentIntake);
            Func<string, Task> setCreateUserName = async (string userId) =>
            {
                if (!string.IsNullOrEmpty(userId))
                    result.CreateUserName = (await _userService.Get(GetAuthorizationSettings(), _settings.Value.AuthorizationDomainId.Value, Guid.Parse(userId)))?.Name ?? string.Empty;
            };
            Func<string, Task> setUpdateUserName = async (string userId) =>
            {
                if (!string.IsNullOrEmpty(userId))
                    result.UpdateUserName = (await _userService.Get(GetAuthorizationSettings(), _settings.Value.AuthorizationDomainId.Value, Guid.Parse(userId)))?.Name ?? string.Empty;
            };
            Func<Task> setStatusDescription = async () =>
            {
                result.StatusDescription = await paymentIntake.GetStatusDescription(settings);
            };
            await Task.WhenAll(
                setCreateUserName(paymentIntake.CreateUserId),
                setUpdateUserName(paymentIntake.UpdateUserId),
                setStatusDescription());
            result.Loan = mapper.Map<Loan>(await paymentIntake.GetLaon(settings));
            return result;
        }

        [HttpPost("Payment")]
        [Authorize(Constants.POLICY_LOAN_EDIT)]
        public async Task<IActionResult> Commit()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                await _paymentIntakeSaver.Commit(
                    settings,
                    PaymentIntakeStatus.New,
                    PaymentIntakeStatus.Processed,
                    PaymentStatus.Unprocessed,
                    (await GetCurrentUserId()).Value.ToString("D"));
                result = Ok();
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("commit-payment-intake", start, result);
            }
            return result;
        }
    }
}
