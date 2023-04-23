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
using System.Net;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;
using ConfigAPI = BrassLoon.Interface.Config;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : APIControllerBase
    {
        private readonly ConfigAPI.ILookupService _lookupService;
        private readonly ConfigAPI.IItemService _itemService;

        public LookupController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ConfigAPI.ILookupService lookupService,
            ConfigAPI.IItemService itemService)
            : base(settings, settingsFactory, userService, logger)
        {
            _lookupService = lookupService;
            _itemService = itemService;
        }

        [NonAction]
        private async Task<ConfigAPI.Models.Item> GetIndexItem(ConfigAPI.ISettings settings)
        {
            try
            {
                return await _itemService.GetByCode(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.LookupIndexConfigurationCode);
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;
                else
                    throw;
            }
        }

        [NonAction]
        private List<string> GetIndex(ConfigAPI.Models.Item item)
        {
            List<string> codes = new List<string>();
            if (item?.Data != null)
            {
                foreach (string code in item.Data)
                {
                    codes.Add(code);
                }
            }
            return codes;
        }

        [NonAction]
        private async Task<ConfigAPI.Models.Lookup> GetLookupByCode(ConfigAPI.ISettings settings, string code)
        {
            try
            {
                return await _lookupService.GetByCode(settings, _settings.Value.ConfigDomainId.Value, code);
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;
                else
                    throw;
            }
        }

        [HttpGet("/api/LookupIndex")]
        [Authorize(Constants.POLICY_LOOKUP_EDIT)]
        [ProducesResponseType(typeof(string[]), 200)]
        public async Task<IActionResult> Get()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null)
                {
                    ConfigurationSettings settings = GetConfigurationSettings();
                    List<string> codes = GetIndex(await GetIndexItem(settings));
                    codes.Sort();
                    result = Ok(codes);
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
                await WriteMetrics("get-lookup-index", start, result);
            }
            return result;
        }

        [HttpGet("{code}")]
        [Authorize(Constants.POLICY_LOOKUP_EDIT)]
        [ProducesResponseType(typeof(Lookup), 200)]
        public async Task<IActionResult> Get([FromRoute] string code)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && string.IsNullOrEmpty(code))
                    result = BadRequest("Missing code parameter value");
                if (result == null)
                {
                    ConfigurationSettings settings = GetConfigurationSettings();
                    Task<ConfigAPI.Models.Item> lookupIndexTask = GetIndexItem(settings);
                    ConfigAPI.Models.Lookup lookup = await GetLookupByCode(settings, code);
                    List<string> lookupIndex = GetIndex(await lookupIndexTask);
                    if (lookup == null || !lookupIndex.Any(k => string.Equals(k, lookup.Code, StringComparison.OrdinalIgnoreCase)))
                    {
                        result = NotFound();
                    }
                    else
                    {
                        IMapper mapper = MapperConfiguration.CreateMapper();
                        result = Ok(mapper.Map<Lookup>(lookup));
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
                await WriteMetrics("get-lookup-by-code", start, result);
            }
            return result;
        }

        [HttpPut("{code}")]
        [Authorize(Constants.POLICY_LOOKUP_EDIT)]
        [ProducesResponseType(typeof(Lookup), 200)]
        public async Task<IActionResult> Save([FromRoute] string code, [FromBody] Dictionary<string, string> data)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null && string.IsNullOrEmpty(code))
                    result = BadRequest("Missing code parameter value");
                if (result == null)
                {
                    ConfigurationSettings settings = GetConfigurationSettings();
                    Task updateIndex = UpdateIndex(settings, code);
                    ConfigAPI.Models.Lookup lookup = await _lookupService.Save(settings, _settings.Value.ConfigDomainId.Value, code, data);                    
                    await updateIndex;                                                               
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(mapper.Map<Lookup>(lookup));
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
                await WriteMetrics("save-lookup-by-code", start, result);
            }
            return result;
        }

        [NonAction]
        private async Task UpdateIndex(ConfigAPI.ISettings settings, string code)
        {
            List<string> lookupIndex = GetIndex(await GetIndexItem(settings));
            if (!lookupIndex.Any(i => string.Equals(i, code, StringComparison.OrdinalIgnoreCase)))
            {
                lookupIndex.Add(code);
                await _itemService.Save(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.LookupIndexConfigurationCode, lookupIndex);
            }
        }
    }
}
