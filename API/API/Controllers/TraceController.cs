using AutoMapper;
using BrassLoon.Interface.Log.Models;
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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraceController : APIControllerBase
    {
        private readonly Log.ITraceService _traceService;

        public TraceController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            Log.ITraceService traceService)
            : base(settings, settingsFactory, userService, logger)
        {
            _traceService = traceService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(Trace[]), 200)]
        [Authorize(Constants.POLICY_LOG_READ)]
        public async Task<IActionResult> Search([FromQuery] DateTime? maxTimestamp = null, [FromQuery] string eventCode = null)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && !maxTimestamp.HasValue)
                    result = BadRequest("Missing maxTimestamp parameter value");
                if (result == null && string.IsNullOrEmpty(eventCode))
                    result = BadRequest("Missing event code parameter value");
                if (result == null && !_settings.Value.LogDomainId.HasValue)
                    result = StatusCode(StatusCodes.Status500InternalServerError, "Missing log domain id configuration value");
                if (result == null)
                {
                    Log.ISettings settings = _settingsFactory.CreateLog(_settings.Value);
                    IMapper mapper = new Mapper(MapperConfiguration.Get());
                    List<Trace> traces = (await _traceService.Search(settings, _settings.Value.LogDomainId.Value, maxTimestamp.Value, eventCode))
                        .Select(m => mapper.Map<Trace>(m))
                        .ToList();
                    result = Ok(traces);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-trace-search", start, result, new Dictionary<string, string> { { nameof(maxTimestamp), maxTimestamp?.ToString("o") } });
            }
            return result;
        }

        [HttpGet("/api/TraceEventCode")]
        [ProducesResponseType(typeof(string[]), 200)]
        [ResponseCache(Duration = 150, Location = ResponseCacheLocation.Client)]
        [Authorize(Constants.POLICY_LOG_READ)]
        public async Task<IActionResult> GetEventCode()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && !_settings.Value.LogDomainId.HasValue)
                    result = StatusCode(StatusCodes.Status500InternalServerError, "Missing log domain id configuration value");
                if (result == null)
                {
                    Log.ISettings settings = _settingsFactory.CreateLog(_settings.Value);
                    result = Ok(
                        await _traceService.GetEventCodes(settings, _settings.Value.LogDomainId.Value)
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
                await WriteMetrics("get-trace-event-codes", start, result);
            }
            return result;
        }
    }
}
