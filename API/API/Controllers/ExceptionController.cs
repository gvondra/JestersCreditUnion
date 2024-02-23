using AutoMapper;
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
using Log = BrassLoon.Interface.Log;
using Models = BrassLoon.Interface.Log.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : APIControllerBase
    {
        private readonly Log.IExceptionService _exceptionService;

        public ExceptionController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<ExceptionController> logger,
            Log.IExceptionService exceptionService)
            : base(settings, settingsFactory, userService, logger)
        {
            _exceptionService = exceptionService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_LOG_READ)]
        public async Task<IActionResult> Search([FromQuery] DateTime? maxTimestamp)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (!maxTimestamp.HasValue)
                {
                    result = BadRequest("Missing maxTimestamp parameter value");
                }
                else if (!_settings.Value.LogDomainId.HasValue)
                {
                    result = StatusCode(StatusCodes.Status500InternalServerError, "Missing log domain id configuration value");
                }
                else
                {
                    Log.ISettings settings = _settingsFactory.CreateLog(_settings.Value);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    IEnumerable<Models.Exception> exceptions = (await _exceptionService.Search(settings, _settings.Value.LogDomainId.Value, maxTimestamp.Value))
                        .Select<Log.Models.Exception, Models.Exception>(mapper.Map<Models.Exception>);
                    result = Ok(exceptions);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-exceptions-search", start, result, new Dictionary<string, string> { { nameof(maxTimestamp), maxTimestamp?.ToString("o") } });
            }
            return result;
        }
    }
}
