using AutoMapper;
using BrassLoon.Interface.WorkTask.Models;
using JestersCreditUnion.CommonAPI;
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
    }
}
