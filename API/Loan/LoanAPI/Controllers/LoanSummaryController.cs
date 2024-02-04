using AutoMapper;
using BrassLoon.Interface.Authorization;
using JestersCreditUnion.Loan.Framework.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Loan.Models;

namespace LoanAPI.Controllers
{
    [Route("api/Reporting/[controller]")]
    [ApiController]
    public class LoanSummaryController : LoanApiControllerBase
    {
        private readonly ILoanSummaryFactory _factory;
        private readonly ILoanSummaryBuilder _builder;

        public LoanSummaryController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            IUserService userService,
            ILogger<LoanSummaryController> logger,
            ILoanSummaryFactory factory,
            ILoanSummaryBuilder builder)
            : base(settings, settingsFactory, userService, logger)
        {
            _factory = factory;
            _builder = builder;
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [HttpGet("Open")]
        public async Task<IActionResult> GetOpenLoanSummary()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                IEnumerable<IOpenLoanSummary> innerItems = await _factory.Get(settings);
                ILoanSummary loanSummary = _builder.BuildOpenLoanSummary(innerItems);
                IMapper mapper = MapperConfiguration.CreateMapper();
                OpenLoanSummary openLoanSummary = mapper.Map<OpenLoanSummary>(loanSummary);
                result = Ok(openLoanSummary);
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-open-ln-summary", start, result);
            }
            return result;
        }
    }
}
