using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace API.Controllers
{
    [Route("api/Loan/{id}/[controller]")]
    [ApiController]
    public class AmortizationController : APIControllerBase
    {
        private readonly IAmortizationBuilder _amortizationBuilder;
        private readonly ILoanFactory _loanFactory;

        public AmortizationController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<AmortizationController> logger,
            IAmortizationBuilder amortizationBuilder,
            ILoanFactory loanFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _amortizationBuilder = amortizationBuilder;
            _loanFactory = loanFactory;
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [ProducesResponseType(typeof(AmortizationItem[]), 200)]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ILoan loan = null;
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing loan id parameter value");
                }
                else
                {
                    loan = await _loanFactory.Get(GetCoreSettings(), id.Value);
                    if (loan == null)
                        result = NotFound();
                }
                if (result == null && loan != null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        (await _amortizationBuilder.Build(GetCoreSettings(), loan))
                        .Select<IAmortizationItem, AmortizationItem>(am => mapper.Map<AmortizationItem>(am)));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-amortization", start, result);
            }
            return result;
        }
    }
}
