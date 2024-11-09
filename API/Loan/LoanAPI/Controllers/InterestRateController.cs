using BrassLoon.Interface.Config;
using BrassLoon.Interface.Config.Models;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Loan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestRateController : LoanApiControllerBase
    {
        private readonly IItemService _itemService;

        public InterestRateController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<InterestRateController> logger,
            IItemService itemService)
            : base(settings, settingsFactory, userService, logger)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_INTEREST_RATE_CONFIGURE)]
        [ProducesResponseType(typeof(InterestRateConfiguration), 200)]
        public async Task<IActionResult> Get()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ConfigurationSettings settings = GetConfigurationSettings();
                Item item = await _itemService.GetByCode(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.InterestRateConfigurationCode);
                result = Ok(Map(item?.Data));
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-interest-rate-config", start, result);
            }
            return result;
        }

        [HttpPut]
        [Authorize(Constants.POLICY_INTEREST_RATE_CONFIGURE)]
        [ProducesResponseType(typeof(InterestRateConfiguration), 200)]
        public async Task<IActionResult> Save([FromBody] InterestRateConfiguration interestRateConfiguration)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ConfigurationSettings settings = GetConfigurationSettings();
                Item item = await _itemService.Save(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.InterestRateConfigurationCode, interestRateConfiguration);
                result = Ok(Map(item?.Data));
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("save-interest-rate-config", start, result);
            }
            return result;
        }

        private static InterestRateConfiguration Map(dynamic dataObject)
        {
            IDictionary<string, object> data = null;
            if (dataObject != null && typeof(IDictionary<string, object>).IsAssignableFrom(dataObject.GetType()))
                data = (IDictionary<string, object>)dataObject;
            else if (dataObject != null && typeof(IEnumerable<KeyValuePair<string, JToken>>).IsAssignableFrom(dataObject.GetType()))
                data = ((IEnumerable<KeyValuePair<string, JToken>>)dataObject).Select(kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value)).ToDictionary();
            if (data == null)
                data = new Dictionary<string, object>();
            object inflationRate;
            object operationsRate;
            object lossRate;
            object incentiveRate;
            object otherRate;
            object totalRate;
            object minimumRate;
            object maximumRate;
            object otherRateDescription;
            if (!data.TryGetValue("InflationRate", out inflationRate))
                inflationRate = 0.0M;
            if (!data.TryGetValue("OperationsRate", out operationsRate))
                operationsRate = 0.0M;
            if (!data.TryGetValue("LossRate", out lossRate))
                lossRate = 0.0M;
            if (!data.TryGetValue("IncentiveRate", out incentiveRate))
                incentiveRate = 0.0M;
            if (!data.TryGetValue("OtherRate", out otherRate))
                otherRate = 0.0M;
            if (!data.TryGetValue("TotalRate", out totalRate))
                totalRate = 0.0M;
            if (!data.TryGetValue("MinimumRate", out minimumRate))
                minimumRate = 0.0M;
            if (!data.TryGetValue("MaximumRate", out maximumRate))
                maximumRate = 0.0M;
            if (!data.TryGetValue("OtherRateDescription", out otherRateDescription))
                otherRateDescription = string.Empty;
            return new InterestRateConfiguration
            {
                InflationRate = inflationRate != null ? Convert.ToDecimal(inflationRate, CultureInfo.InvariantCulture) : null,
                OperationsRate = operationsRate != null ? Convert.ToDecimal(operationsRate, CultureInfo.InvariantCulture) : null,
                LossRate = lossRate != null ? Convert.ToDecimal(lossRate, CultureInfo.InvariantCulture) : null,
                IncentiveRate = incentiveRate != null ? Convert.ToDecimal(incentiveRate, CultureInfo.InvariantCulture) : null,
                OtherRate = otherRate != null ? Convert.ToDecimal(otherRate, CultureInfo.InvariantCulture) : null,
                TotalRate = totalRate != null ? Convert.ToDecimal(totalRate, CultureInfo.InvariantCulture) : null,
                MinimumRate = minimumRate != null ? Convert.ToDecimal(minimumRate, CultureInfo.InvariantCulture) : null,
                MaximumRate = maximumRate != null ? Convert.ToDecimal(maximumRate, CultureInfo.InvariantCulture) : null,
                OtherRateDescription = otherRateDescription?.ToString() ?? string.Empty
            };
        }
    }
}
