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
            ILogger<WorkTaskController> logger,
            WorkTaskAPI.IWorkTaskService workTaskService)
            : base(settings, settingsFactory, userService, logger)
        {
            _workTaskService = workTaskService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_WORKTASK_READ)]
        [ProducesResponseType(typeof(WorkTask[]), 200)]
        public async Task<IActionResult> Search([FromQuery] short? referenceType, [FromQuery] string referenceValue, [FromQuery] bool? includeClosed = null)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                WorkTaskSettings settings = GetWorkTaskSettings();
                IMapper mapper = MapperConfiguration.CreateMapper();
                if (referenceType.HasValue && !string.IsNullOrEmpty(referenceValue))
                {
                    result = Ok(
                        (await _workTaskService.GetByContext(settings, _settings.Value.WorkTaskDomainId.Value, referenceType.Value, referenceValue, includeClosed))
                        .Select(wt => mapper.Map<WorkTask>(wt))
                        .ToList()
                        );
                }
                else
                {
                    result = Ok(Array.Empty<WorkTask>());
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
                await WriteMetrics("search-worktasks", start, result);
            }
            return result;
        }

        [HttpGet("/api/WorkGroup/{workGroupId}/WorkTask")]
        [Authorize(Constants.POLICY_WORKTASK_READ)]
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
                        .ToList()
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
        [Authorize(Constants.POLICY_WORKTASK_CLAIM)]
        [ProducesResponseType(typeof(ClaimWorkTaskResponse), 200)]
        public async Task<IActionResult> Claim([FromRoute] Guid? id, [FromQuery] string assignToUserId, [FromQuery] DateTime? assignedDate = null)
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
                    WorkTaskAPI.Models.ClaimWorkTaskResponse response = await _workTaskService.Claim(settings, _settings.Value.WorkTaskDomainId.Value, id.Value, assignToUserId ?? string.Empty, assignedDate);
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

        [HttpPut("{id}")]
        [Authorize(Constants.POLICY_WORKTASK_EDIT)]
        [ProducesResponseType(typeof(WorkTask), 200)]
        public async Task<IActionResult> Update([FromRoute] Guid? id, [FromBody] WorkTask workTask)
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
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkTask innerWorkTask = mapper.Map<WorkTaskAPI.Models.WorkTask>(workTask);
                    innerWorkTask.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkTask.WorkTaskId = id.Value;
                    innerWorkTask = await _workTaskService.Update(settings, innerWorkTask);
                    result = Ok(mapper.Map<WorkTask>(innerWorkTask));
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
                await WriteMetrics("update-worktask", start, result);
            }
            return result;
        }

        [HttpPatch]
        [Authorize(Constants.POLICY_WORKTASK_EDIT)]
        [ProducesResponseType(typeof(WorkTask[]), 200)]
        public async Task<IActionResult> Patch([FromBody] List<Dictionary<string, object>> patchData)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                WorkTaskSettings settings = GetWorkTaskSettings();
                IMapper mapper = MapperConfiguration.CreateMapper();
                result = Ok(
                    (await _workTaskService.Patch(settings, _settings.Value.WorkTaskDomainId.Value, patchData))
                    .Select(wt => mapper.Map<WorkTask>(wt))
                    .ToList()
                    );
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
                await WriteMetrics("patch-worktask", start, result);
            }
            return result;
        }
    }
}
