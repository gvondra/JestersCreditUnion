using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Loan.Framework.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;
using ConfigAPI = BrassLoon.Interface.Config;

namespace LoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTaskConfigurationController : LoanApiControllerBase
    {
        private readonly ConfigAPI.IItemService _itemService;
        public WorkTaskConfigurationController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<WorkTaskConfigurationController> logger,
            ConfigAPI.IItemService itemService)
            : base(settings, settingsFactory, userService, logger)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(WorkTaskConfiguration), 200)]
        public async Task<IActionResult> Get()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ConfigurationSettings settings = GetConfigurationSettings();
                dynamic data = await _itemService.GetDataByCode(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.WorkTaskConfigurationCode);
                WorkTaskConfiguration workTaskConfiguration = new WorkTaskConfiguration();
                if (data != null)
                {
                    workTaskConfiguration.NewLoanApplicationTaskTypeCode = data[WorkTaskConfigurationFields.NewLoanApplicationTaskTypeCode] ?? string.Empty;
                    workTaskConfiguration.SendDenialCorrespondenceTaskTypeCode = data[WorkTaskConfigurationFields.SendDenialCorrespondenceTaskTypeCode] ?? string.Empty;
                    workTaskConfiguration.DiburseFundsTaskTypeCode = data[WorkTaskConfigurationFields.DiburseFundsTaskTypeCode] ?? string.Empty;
                }
                result = Ok(workTaskConfiguration);
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
                await WriteMetrics("get-worktask-configuration", start, result);
            }
            return result;
        }

        [HttpPut]
        [Authorize(Constants.POLICY_WORKTASK_TYPE_READ)]
        [ProducesResponseType(typeof(WorkTaskConfiguration), 200)]
        public async Task<IActionResult> Save([FromBody] WorkTaskConfiguration workTaskConfiguration)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ConfigurationSettings settings = GetConfigurationSettings();
                _ = await _itemService.Save(settings, _settings.Value.ConfigDomainId.Value, _settings.Value.WorkTaskConfigurationCode, workTaskConfiguration);
                result = Ok(workTaskConfiguration);
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
                await WriteMetrics("save-worktask-configuration", start, result);
            }
            return result;
        }
    }
}
