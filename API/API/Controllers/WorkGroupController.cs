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
    public class WorkGroupController : APIControllerBase
    {
        private readonly WorkTaskAPI.IWorkGroupService _workGroupService;

        public WorkGroupController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            WorkTaskAPI.IWorkGroupService workGroupService)
            : base(settings, settingsFactory, userService, logger)
        {
            _workGroupService = workGroupService;
        }

        [HttpGet()]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(List<WorkGroup>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] string userId = null)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    List<WorkTaskAPI.Models.WorkGroup> innerWorkGroups;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        innerWorkGroups = await _workGroupService.GetByMemberUserId(settings, _settings.Value.WorkTaskDomainId.Value, userId);
                    }
                    else
                    {
                        innerWorkGroups = await _workGroupService.GetAll(settings, _settings.Value.WorkTaskDomainId.Value);
                    }
                    result = Ok(
                        innerWorkGroups.Select<WorkTaskAPI.Models.WorkGroup, WorkGroup>(t => mapper.Map<WorkGroup>(t))
                        );
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-all-work-groups", start, result);
            }
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(WorkGroup), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
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
                    WorkTaskAPI.Models.WorkGroup innerWorkGroup = await _workGroupService.Get(settings, _settings.Value.WorkTaskDomainId.Value, id.Value);
                    result = Ok(
                        mapper.Map<WorkGroup>(innerWorkGroup)
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
                await WriteMetrics("get-work-group", start, result);
            }
            return result;
        }

        [NonAction]
        private IActionResult ValidateRequest(WorkGroup workGroup)
        {
            IActionResult result = null;
            if (result == null && workGroup == null)
                result = BadRequest("Missing work group body");
            if (result == null && string.IsNullOrEmpty(workGroup?.Title))
                result = BadRequest("Missing work group title value");
            return result;
        }

        [HttpPost()]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        [ProducesResponseType(typeof(WorkGroup), 200)]
        public async Task<IActionResult> Create([FromBody] WorkGroup workGroup)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null)
                    result = ValidateRequest(workGroup);
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkGroup innerWorkGroup = mapper.Map<WorkTaskAPI.Models.WorkGroup>(workGroup);
                    innerWorkGroup.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkGroup = await _workGroupService.Create(settings, innerWorkGroup);
                    await this.ApplyTaskTypeLinkChanges(innerWorkGroup.WorkGroupId.Value, innerWorkGroup.WorkTaskTypeIds, workGroup.WorkTaskTypeIds);
                    innerWorkGroup.WorkTaskTypeIds = workGroup.WorkTaskTypeIds;
                    result = Ok(mapper.Map<WorkGroup>(innerWorkGroup));
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
                await WriteMetrics("create-work-group", start, result);
            }
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        [ProducesResponseType(typeof(WorkGroup), 200)]
        public async Task<IActionResult> Update([FromRoute] Guid? id, [FromBody] WorkGroup workGroup)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null)
                    result = ValidateRequest(workGroup);
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkGroup innerWorkGroup = mapper.Map<WorkTaskAPI.Models.WorkGroup>(workGroup);
                    innerWorkGroup.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkGroup = await _workGroupService.Update(settings, innerWorkGroup);
                    await this.ApplyTaskTypeLinkChanges(innerWorkGroup.WorkGroupId.Value, innerWorkGroup.WorkTaskTypeIds, workGroup.WorkTaskTypeIds);
                    innerWorkGroup.WorkTaskTypeIds = workGroup.WorkTaskTypeIds;
                    result = Ok(mapper.Map<WorkGroup>(innerWorkGroup));
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
                await WriteMetrics("update-work-group", start, result);
            }
            return result;
        }

        [HttpPost("{id}/WorkTaskType")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        public async Task<IActionResult> AddWorkTaskTypeLink([FromRoute] Guid? id, [FromQuery] Guid? workTaskTypeId)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null && (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing work task type id parameter value");
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    await _workGroupService.AddWorkTaskTypeLink(settings, _settings.Value.WorkTaskDomainId.Value, id.Value, workTaskTypeId.Value);
                    result = Ok();
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("add-work-group-type-link", start, result);
            }
            return result;
        }

        [HttpDelete("{id}/WorkTaskType")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        public async Task<IActionResult> DeleteWorkTaskTypeLink([FromRoute] Guid? id, [FromQuery] Guid? workTaskTypeId)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null && (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing work task type id parameter value");
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    await _workGroupService.DeleteWorkTaskTypeLink(settings, _settings.Value.WorkTaskDomainId.Value, id.Value, workTaskTypeId.Value);
                    result = Ok();
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("delete-work-group-type-link", start, result);
            }
            return result;
        }

        [NonAction]
        private async Task ApplyTaskTypeLinkChanges(Guid workGroupId, List<Guid> currentWorkTaskIds, List<Guid> targetWorkTaskIds)
        {
            if (currentWorkTaskIds == null)
                currentWorkTaskIds = new List<Guid>();
            if (targetWorkTaskIds == null)
                targetWorkTaskIds = new List<Guid>();
            WorkTaskSettings settings = GetWorkTaskSettings();
            List<Task> tasks = new List<Task>();
            foreach (Guid workTaskId in currentWorkTaskIds.Except(targetWorkTaskIds))
            {
                tasks.Add(_workGroupService.DeleteWorkTaskTypeLink(settings, _settings.Value.WorkTaskDomainId.Value, workGroupId, workTaskId));
            }
            foreach (Guid workTaskId in targetWorkTaskIds.Except(currentWorkTaskIds))
            {
                tasks.Add(_workGroupService.AddWorkTaskTypeLink(settings, _settings.Value.WorkTaskDomainId.Value, workGroupId, workTaskId));
            }
            await Task.WhenAll(tasks);
        }
    }
}
