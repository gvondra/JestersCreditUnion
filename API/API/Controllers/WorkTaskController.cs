using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Models;
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
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTaskController : APIControllerBase
    {
        private readonly WorkTaskAPI.IWorkTaskService _workTaskService;
        
        public WorkTaskController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            WorkTaskAPI.IWorkTaskService workTaskService)
            : base(settings, settingsFactory, userService, logger)
        {
            _workTaskService = workTaskService;
        }

        [HttpGet("/api/WorkGroup/{workGroupId}/WorkTask")]
        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(List<WorkTask>), 200)]
        public async Task<IActionResult> GetByWorkGroupId([FromRoute] Guid? workGroupId, bool? includeClosed = null)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!workGroupId.HasValue || workGroupId.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        (await _workTaskService.GetByWorkGroupId(settings, _settings.Value.WorkTaskDomainId.Value, workGroupId.Value, includeClosed))
                        .Select(wt => mapper.Map<WorkTask>(wt))
                        );
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-worktasks-by-workgroupid", start, result);
            }
            return result;
        }

        [HttpPut("{id}/AssignTo")]
        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(ClaimWorkTaskResponse), 200)]
        public async Task<IActionResult> Claim([FromRoute] Guid? id, [FromQuery] string assignToUserId)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    WorkTaskAPI.Models.ClaimWorkTaskResponse response = await _workTaskService.Claim(settings, _settings.Value.WorkTaskDomainId.Value, id.Value, assignToUserId ?? string.Empty);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(mapper.Map<ClaimWorkTaskResponse>(response));
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("claim-worktask", start, result);
            }
            return result;
        }
    }
}
