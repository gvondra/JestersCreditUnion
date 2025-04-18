﻿using AutoMapper;
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
using Log = BrassLoon.Interface.Log;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricController : APIControllerBase
    {
        private readonly Log.IMetricService _metricService;
        private readonly AuthorizationAPI.IUserService _userService;

        public MetricController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<MetricController> logger,
            Log.IMetricService metricService)
            : base(settings, settingsFactory, userService, logger)
        {
            _metricService = metricService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_LOG_READ)]
        public async Task<IActionResult> Search([FromQuery] DateTime? maxTimestamp, [FromQuery] string eventCode)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (!maxTimestamp.HasValue)
                {
                    result = BadRequest("Missing maxTimestamp parameter value");
                }
                else if (string.IsNullOrEmpty(eventCode))
                {
                    result = BadRequest("Missing event code parameter value");
                }
                else if (!_settings.Value.LogDomainId.HasValue)
                {
                    result = StatusCode(StatusCodes.Status500InternalServerError, "Missing log domain id configuration value");
                }
                else
                {
                    Log.ISettings settings = _settingsFactory.CreateLog(_settings.Value);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    List<Metric> metrics = (await _metricService.Search(settings, _settings.Value.LogDomainId.Value, maxTimestamp.Value, eventCode))
                        .Select(mapper.Map<Metric>)
                        .ToList();
                    await AddUserData(metrics);
                    result = Ok(metrics);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-metrics-search", start, result, new Dictionary<string, string> { { nameof(maxTimestamp), maxTimestamp?.ToString("o") } });
            }
            return result;
        }

        [NonAction]
        private async Task AddUserData(List<Metric> metrics)
        {
            if (metrics != null)
            {
                Dictionary<Guid, AuthorizationAPI.Models.User> userCache = new Dictionary<Guid, AuthorizationAPI.Models.User>();
                AuthorizationAPI.Models.User user;
                AuthorizationAPI.ISettings settings = GetAuthorizationSettings();
                foreach (Metric metric in metrics.Where(m => !string.IsNullOrEmpty(m.Requestor)))
                {
                    if (Guid.TryParse(metric.Requestor.Trim(), out Guid id))
                    {
                        if (!userCache.ContainsKey(id))
                        {
                            user = await _userService.Get(settings, _settings.Value.AuthorizationDomainId.Value, id);
                            if (user != null)
                                userCache.TryAdd(id, user);
                        }
                        if (userCache.TryGetValue(id, out user))
                            metric.RequestorName = user.Name ?? string.Empty;
                    }
                }
            }
        }

        [HttpGet("/api/MetricEventCode")]
        [Authorize(Constants.POLICY_LOG_READ)]
        public async Task<IActionResult> GetEventCodes()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (!_settings.Value.LogDomainId.HasValue)
                {
                    result = StatusCode(StatusCodes.Status500InternalServerError, "Missing log domain id configuration value");
                }
                else
                {
                    Log.ISettings settings = _settingsFactory.CreateLog(_settings.Value);
                    result = Ok(
                        await _metricService.GetEventCodes(settings, _settings.Value.LogDomainId.Value));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-metric-event-codes", start, result);
            }
            return result;
        }
    }
}
