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
    public class WorkTaskCycleController : LoanApiControllerBase
    {
        private readonly IWorkTaskCycleSummaryFactory _workTaskCycleSummaryFactory;

        public WorkTaskCycleController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            IUserService userService,
            ILogger<WorkTaskCycleController> logger,
            IWorkTaskCycleSummaryFactory workTaskCycleSummaryFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _workTaskCycleSummaryFactory = workTaskCycleSummaryFactory;
        }

        [Authorize(Constants.POLICY_LOAN_APPLICATION_READ)]
        [HttpGet("Summary")]
        public async Task<IActionResult> Summary([FromQuery] DateTime? minCreateDate)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (!minCreateDate.HasValue)
                {
                    minCreateDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0, DateTimeKind.Unspecified)
                        .AddMonths(-6);
                }
                CoreSettings settings = GetCoreSettings();
                IEnumerable<WorkTaskCycleSummaryItem> innerItems = await _workTaskCycleSummaryFactory.GetSummary(settings, minCreateDate.Value);
                IMapper mapper = MapperConfiguration.CreateMapper();
                result = Ok(
                    innerItems
                    .Select(i => mapper.Map<Models.WorkTaskCycleSummaryItem>(i))
                    .ToList());
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-wtask-summary", start, result);
            }
            return result;
        }
    }
}
