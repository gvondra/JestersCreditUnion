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
    [Route("api/WorkTaskType/{workTaskTypeId}/Status")]
    [ApiController]
    public class WorkTaskStatusController : APIControllerBase
    {
        private readonly WorkTaskAPI.IWorkTaskTypeService _workTaskTypeService;
        private readonly WorkTaskAPI.IWorkTaskStatusService _workTaskStatusService;

        public WorkTaskStatusController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<WorkTaskStatusController> logger,
            WorkTaskAPI.IWorkTaskTypeService workTaskTypeService,
            WorkTaskAPI.IWorkTaskStatusService workTaskStatusService)
            : base(settings, settingsFactory, userService, logger)
        {
            _workTaskTypeService = workTaskTypeService;
            _workTaskStatusService = workTaskStatusService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(List<WorkTaskStatus>), 200)]
        public async Task<IActionResult> GetAll([FromRoute] Guid? workTaskTypeId)
        {
            IActionResult result = null;
            try
            {
                if (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing work task type id parameter value");
                }
                else
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = await _workTaskTypeService.Get(settings, _settings.Value.WorkTaskDomainId.Value, workTaskTypeId.Value);
                    result = Ok(innerWorkTaskType.Statuses.Select(mapper.Map<WorkTaskStatus>));
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
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(WorkTaskStatus), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid? workTaskTypeId, [FromRoute] Guid? id)
        {
            IActionResult result;
            try
            {
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing id parameter value");
                }
                else if (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing work task type id parameter value");
                }
                else
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = await _workTaskTypeService.Get(settings, _settings.Value.WorkTaskDomainId.Value, workTaskTypeId.Value);
                    WorkTaskAPI.Models.WorkTaskStatus innerWorkTaskStatus = innerWorkTaskType.Statuses.Find(sts => sts.WorkTaskStatusId.Value == id.Value);
                    if (innerWorkTaskStatus == null)
                    {
                        result = NotFound();
                    }
                    else
                    {
                        IMapper mapper = MapperConfiguration.CreateMapper();
                        result = Ok(
                            mapper.Map<WorkTaskStatus>(innerWorkTaskStatus));
                    }
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
            return result;
        }

        [NonAction]
        private IActionResult ValidateRequest(WorkTaskStatus workTaskStatus)
        {
            IActionResult result = null;
            if (workTaskStatus == null)
                result = BadRequest("Missing work task status body");
            else if (string.IsNullOrEmpty(workTaskStatus.Name))
                result = BadRequest("Missing work task status name value");
            else if (!workTaskStatus.IsClosedStatus.HasValue)
                result = BadRequest("Missing work task status is closed value");
            return result;
        }

        [HttpPost]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        [ProducesResponseType(typeof(WorkTaskStatus), 200)]
        public async Task<IActionResult> Create([FromRoute] Guid? workTaskTypeId, [FromBody] WorkTaskStatus workTaskStatus)
        {
            IActionResult result;
            try
            {
                if (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty))
                    result = BadRequest("Missing work task type id parameter value");
                else
                    result = ValidateRequest(workTaskStatus);
                if (result == null && string.IsNullOrEmpty(workTaskStatus?.Code))
                    result = BadRequest("Missing work task status code value");
                if (result == null && workTaskTypeId.HasValue)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkTaskStatus innerWorkTaskStatus = mapper.Map<WorkTaskAPI.Models.WorkTaskStatus>(workTaskStatus);
                    innerWorkTaskStatus.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkTaskStatus.WorkTaskTypeId = workTaskTypeId.Value;
                    innerWorkTaskStatus = await _workTaskStatusService.Create(settings, innerWorkTaskStatus);
                    result = Ok(
                        mapper.Map<WorkTaskStatus>(innerWorkTaskStatus));
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
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(WorkTaskStatus), 200)]
        public async Task<IActionResult> Update([FromRoute] Guid? workTaskTypeId, [FromRoute] Guid? id, [FromBody] WorkTaskStatus workTaskStatus)
        {
            IActionResult result;
            try
            {
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                    result = BadRequest("Missing id parameter value");
                else if (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty))
                    result = BadRequest("Missing work task type id parameter value");
                else
                    result = ValidateRequest(workTaskStatus);
                if (result == null && id.HasValue && workTaskTypeId.HasValue)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkTaskStatus innerWorkTaskStatus = mapper.Map<WorkTaskAPI.Models.WorkTaskStatus>(workTaskStatus);
                    innerWorkTaskStatus.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkTaskStatus.WorkTaskTypeId = workTaskTypeId.Value;
                    innerWorkTaskStatus.WorkTaskStatusId = id.Value;
                    innerWorkTaskStatus = await _workTaskStatusService.Update(settings, innerWorkTaskStatus);
                    result = Ok(
                        mapper.Map<WorkTaskStatus>(innerWorkTaskStatus));
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
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Constants.POLICY_BL_AUTH)]
        public async Task<IActionResult> Delete([FromRoute] Guid? workTaskTypeId, [FromRoute] Guid? id)
        {
            IActionResult result;
            try
            {
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing id parameter value");
                }
                else if (!workTaskTypeId.HasValue || workTaskTypeId.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing work task type id parameter value");
                }
                else
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    await _workTaskStatusService.Delete(settings, _settings.Value.WorkTaskDomainId.Value, workTaskTypeId.Value, id.Value);
                    result = Ok();
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            return result;
        }
    }
}
