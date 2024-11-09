using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrassLoon.Interface.Authorization;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Loan.Framework.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models = JestersCreditUnion.Interface.Loan.Models;

namespace LoanAPI.Controllers
{
    [Route("api/Reporting/Loan")]
    [ApiController]
    public class LoanReportController : LoanApiControllerBase
    {
        private readonly ILoanBalanceFactory _loanBalanceFactory;

        public LoanReportController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            IUserService userService,
            ILogger<LoanReportController> logger,
            ILoanBalanceFactory loanBalanceFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanBalanceFactory = loanBalanceFactory;
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [HttpGet("PastDue")]
        public async Task<IActionResult> GetPastDue([FromQuery] short minDays = 60)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                IEnumerable<LoanPastDue> innerItems = await _loanBalanceFactory.GetLoansPastDue(settings, minDays);
                IMapper mapper = MapperConfiguration.CreateMapper();
                result = Ok(innerItems.Select<LoanPastDue, Models.LoanPastDue>(l => mapper.Map<Models.LoanPastDue>(l)).ToList());
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-rpt-ln-past-due", start, result);
            }
            return result;
        }
    }
}
