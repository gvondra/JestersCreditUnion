using AutoMapper;
using BrassLoon.Interface.Authorization;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Loan.Framework.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = JestersCreditUnion.Interface.Loan.Models;

namespace LoanAPI.Controllers
{
    [Route("api/Reporting/[controller]")]
    [ApiController]
    public class LoanApplicationSummaryController : LoanApiControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;

        public LoanApplicationSummaryController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            IUserService userService,
            ILogger<LoanApplicationSummaryController> logger,
            ILoanApplicationFactory loanApplicationFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
        }

        [Authorize(Constants.POLICY_LOAN_APPLICATION_READ)]
        [HttpGet]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime? minApplicationDate)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (!minApplicationDate.HasValue)
                {
                    minApplicationDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                        .AddMonths(-6);
                }
                CoreSettings settings = GetCoreSettings();
                IEnumerable<LoanApplicationSummaryItem> innerItems = await _loanApplicationFactory.GetLoanApplicationSummary(settings, minApplicationDate.Value);
                IMapper mapper = MapperConfiguration.CreateMapper();
                result = Ok(
                    innerItems
                    .Select(i => mapper.Map<Models.LoanApplicationSummaryItem>(i))
                    .ToList());
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-ln-app-summary", start, result);
            }
            return result;
        }
    }
}
