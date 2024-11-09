using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BrassLoon.Interface.Authorization;
using JestersCreditUnion.CommonAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI
{
    public abstract class LoanApiControllerBase : CommonControllerBase
    {
#pragma warning disable SA1401 // Fields should be private
        protected readonly IOptions<Settings> _settings;
        protected readonly ISettingsFactory _settingsFactory;
#pragma warning restore SA1401 // Fields should be private
        private CoreSettings _coreSettings;
        private AuthorizationSettings _authorizationSettings;
        private ConfigurationSettings _configurationSettings;

        protected LoanApiControllerBase(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            IUserService userService,
            ILogger logger)
            : base(userService, logger)
        {
            _settings = settings;
            _settingsFactory = settingsFactory;
        }

        protected async Task WriteMetrics(string eventCode, double? magnitude, IActionResult actionResult = null, Dictionary<string, string> data = null)
        {
            await WriteMetrics(
                _settingsFactory.CreateAuthorization(),
                _settings.Value.AuthorizationDomainId.Value,
                eventCode,
                magnitude: magnitude,
                actionResult: actionResult,
                data: data);
        }

        protected Task WriteMetrics(string eventCode, DateTime? startTime, IActionResult actionResult = null, Dictionary<string, string> data = null)
        {
            double? magnitude = null;
            if (startTime.HasValue)
            {
                startTime = startTime.Value.ToUniversalTime();
                magnitude = DateTime.UtcNow.Subtract(startTime.Value).TotalSeconds;
            }
            return WriteMetrics(eventCode, magnitude, actionResult, data);
        }

        protected virtual Task<Guid?> GetCurrentUserId()
            => GetCurrentUserId(GetAuthorizationSettings(), _settings.Value.AuthorizationDomainId.Value);

        protected virtual Task<string> GetCurrentUserEmailAddress(AuthorizationAPI.ISettings settings)
            => GetCurrentUserEmailAddress(settings, _settings.Value.AuthorizationDomainId.Value);

        protected virtual CoreSettings GetCoreSettings()
        {
            if (_coreSettings == null)
                _coreSettings = _settingsFactory.CreateCore();
            return _coreSettings;
        }

        protected virtual AuthorizationSettings GetAuthorizationSettings()
        {
            if (_authorizationSettings == null)
                _authorizationSettings = _settingsFactory.CreateAuthorization();
            return _authorizationSettings;
        }

        protected virtual ConfigurationSettings GetConfigurationSettings()
        {
            if (_configurationSettings == null)
                _configurationSettings = _settingsFactory.CreateConfiguration();
            return _configurationSettings;
        }
    }
}
