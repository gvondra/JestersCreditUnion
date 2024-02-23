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
using ConfigAPI = BrassLoon.Interface.Config;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTaskTypeController : APIControllerBase
    {
        private readonly WorkTaskAPI.IWorkTaskTypeService _workTaskTypeService;
        private readonly ConfigAPI.IItemService _itemService;

        public WorkTaskTypeController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<WorkTaskTypeController> logger,
            WorkTaskAPI.IWorkTaskTypeService workTaskTypeService,
            ConfigAPI.IItemService itemService)
            : base(settings, settingsFactory, userService, logger)
        {
            _workTaskTypeService = workTaskTypeService;
            _itemService = itemService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(List<WorkTaskType>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] string code)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                WorkTaskSettings settings = GetWorkTaskSettings();
                IMapper mapper = MapperConfiguration.CreateMapper();
                if (!string.IsNullOrEmpty(code))
                {
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = await _workTaskTypeService.GetByCode(settings, _settings.Value.WorkTaskDomainId.Value, code);
                    if (innerWorkTaskType == null)
                    {
                        result = Ok(null);
                    }
                    else
                    {
                        result = Ok(
                            mapper.Map<WorkTaskType>(innerWorkTaskType));
                    }
                }
                else
                {
                    result = Ok(
                        (await _workTaskTypeService.GetAll(settings, _settings.Value.WorkTaskDomainId.Value))
                        .Select(mapper.Map<WorkTaskType>));
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
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing id parameter value");
                }
                else
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    WorkTaskAPI.Models.WorkTaskType innerWorkTaskType = await _workTaskTypeService.Get(settings, _settings.Value.WorkTaskDomainId.Value, id.Value);
                    if (innerWorkTaskType == null)
                        result = NotFound();
                    if (result == null)
                    {
                        IMapper mapper = MapperConfiguration.CreateMapper();
                        result = Ok(
                            mapper.Map<WorkTaskType>(innerWorkTaskType));
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
                if (!workGroupId.HasValue || workGroupId.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing work group id parameter value");
                }
                else
                {
                    WorkTaskSettings settings = GetWorkTaskSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        (await _workTaskTypeService.GetByWorkGroupId(settings, _settings.Value.WorkTaskDomainId.Value, workGroupId.Value))
                        .Select(mapper.Map<WorkTaskType>));
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
                await WriteMetrics("get-worktask-type-by-work-group-id", start, result);
            }
            return result;
        }

        [NonAction]
        private IActionResult ValidateRequest(WorkTaskType workTaskType)
        {
            IActionResult result = null;
            if (workTaskType == null)
                result = BadRequest("Missing work task type body");
            else if (string.IsNullOrEmpty(workTaskType.Title))
                result = BadRequest("Missing work task type title value");
            return result;
        }

        [HttpPost]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_EDIT)]
        [ProducesResponseType(typeof(WorkTaskType), 200)]
        public async Task<IActionResult> Create([FromBody] WorkTaskType workTaskType)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
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
                        mapper.Map<WorkTaskType>(innerWorkTaskType));
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
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
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
                        mapper.Map<WorkTaskType>(innerWorkTaskType));
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
                await WriteMetrics("update-worktask-type", start, result);
            }
            return result;
        }

        [HttpGet("/api/WorkTaskTypeCode")]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> CodeLookup([FromQuery] string name)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    result = BadRequest("Missing name parameter value");
                }
                else
                {
                    ConfigurationSettings settings = GetConfigurationSettings();
                    dynamic configData = await _itemService.GetDataByCode(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.WorkTaskConfigurationCode);
                    if (configData != null)
                        result = Ok(configData[name] ?? string.Empty);
                    else
                        result = Ok(null);
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
                await WriteMetrics("lookup-worktask-type-code", start, result);
            }
            return result;
        }
    }
}
