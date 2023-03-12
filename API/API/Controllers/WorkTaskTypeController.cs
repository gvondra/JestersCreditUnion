using AutoMapper;
using JestersCreditUnion.Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AuthorizationAPI = BrassLoon.Interface.Authorization;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;
using JestersCreditUnion.CommonAPI;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTaskTypeController : APIControllerBase
    {
        private readonly WorkTaskAPI.IWorkTaskTypeService _workTaskTypeService;
        public WorkTaskTypeController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            WorkTaskAPI.IWorkTaskTypeService workTaskTypeService)
            : base(settings, settingsFactory, userService, logger)
        {
            _workTaskTypeService = workTaskTypeService;
        }

        [HttpGet()]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(List<WorkTaskType>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] string code)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    if (!string.IsNullOrEmpty(code))
                    {
                        WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = await _workTaskTypeService.GetByCode(settings, _settings.Value.WorkTaskDomainId.Value, code);
                        if (innerWorkTaskType == null)
                            result = Ok(null);
                        else
                            result = Ok(
                                mapper.Map<WorkTaskType>(innerWorkTaskType)
                                );
                    }                    
                    else
                    {
                        result = Ok(
                            (await _workTaskTypeService.GetAll(settings, _settings.Value.WorkTaskDomainId.Value))
                            .Select(t => mapper.Map<WorkTaskType>(t))
                            );
                    }
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-worktask-type-all", start, result);
            }
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(List<WorkTaskType>), 200)]
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
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = await _workTaskTypeService.Get(settings, _settings.Value.WorkTaskDomainId.Value, id.Value);
                    if (innerWorkTaskType == null)
                        result = NotFound();
                    if (result == null)
                    {
                        IMapper mapper = MapperConfiguration.CreateMapper();
                        result = Ok(
                            mapper.Map<WorkTaskType>(innerWorkTaskType)
                            );
                    }
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-worktask-type-by-id", start, result);
            }
            return result;
        }

        [HttpGet("/api/WorkGroup/{workGroupId}/WorkTaskType")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(List<WorkTaskType>), 200)]
        public async Task<IActionResult> GetByWorkGroupId([FromRoute] Guid? workGroupId)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!workGroupId.HasValue || workGroupId.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing work group id parameter value");
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        (await _workTaskTypeService.GetByWorkGroupId(settings, _settings.Value.WorkTaskDomainId.Value, workGroupId.Value))
                        .Select(t => mapper.Map<WorkTaskType>(t))
                        );
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-worktask-type-by-work-group-id", start, result);
            }
            return result;
        }

        [NonAction]
        private IActionResult ValidateRequest(WorkTaskType workTaskType)
        {
            IActionResult result = null;
            if (result == null && workTaskType == null)
                result = BadRequest("Missing work task type body");
            if (result == null && string.IsNullOrEmpty(workTaskType?.Title))
                result = BadRequest("Missing work task type title value");
            return result;
        }

        [HttpPost()]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        [ProducesResponseType(typeof(WorkTaskType), 200)]
        public async Task<IActionResult> Create([FromBody] WorkTaskType workTaskType)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null)
                    result = ValidateRequest(workTaskType);
                if (result == null && string.IsNullOrEmpty(workTaskType.Code))
                    result = BadRequest("Missing work task type code value");
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = mapper.Map<WorkTaskAPI.Models.WorkTaskType>(workTaskType);
                    innerWorkTaskType.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkTaskType = await _workTaskTypeService.Create(settings, innerWorkTaskType);                    
                    result = Ok(
                        mapper.Map<WorkTaskType>(innerWorkTaskType)
                        );
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("create-worktask-type", start, result);
            }
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        [ProducesResponseType(typeof(WorkTaskType), 200)]
        public async Task<IActionResult> Update([FromRoute] Guid? id, [FromBody] WorkTaskType workTaskType)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null)
                    result = ValidateRequest(workTaskType);
                if (result == null)
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = mapper.Map<WorkTaskAPI.Models.WorkTaskType>(workTaskType);
                    innerWorkTaskType.DomainId = _settings.Value.WorkTaskDomainId.Value;
                    innerWorkTaskType = await _workTaskTypeService.Update(settings, innerWorkTaskType);
                    result = Ok(
                        mapper.Map<WorkTaskType>(innerWorkTaskType)
                        );
                }
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                WriteException(ex);
                result = StatusCode((int)ex.StatusCode);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("update-worktask-type", start, result);
            }
            return result;
        }
    }
}
