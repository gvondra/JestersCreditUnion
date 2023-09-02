using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : APIControllerBase
    {
        private readonly AuthorizationAPI.ITokenService _tokenService;

        public TokenController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<TokenController> logger,
            AuthorizationAPI.ITokenService tokenService)
            : base(settings, settingsFactory, userService, logger)
        {
            _tokenService = tokenService;
        }

        [HttpPost()]
        [Authorize(Constants.POLICY_TOKEN_CREATE)]
        public async Task<IActionResult> Create()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (result == null)
                {
                    AuthorizationAPI.ISettings settings = _settingsFactory.CreateAuthorization(_settings.Value, GetUserToken());
                    result = Content(await _tokenService.Create(settings, _settings.Value.AuthorizationDomainId.Value),
                        "text/plain");
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
            finally
            {
                await WriteMetrics("create-token", start, result);
            }
            return result;
        }

        [HttpPost("ClientCredential")]
        public async Task<IActionResult> CreateClientCredential([FromBody] ClientCredential clientCredential)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if ((clientCredential?.ClientId.HasValue ?? false) == false)
                {
                    result = BadRequest("Missing client id value");
                }
                else
                {
                    AuthorizationAPI.ISettings settings = _settingsFactory.CreateAuthorization(_settings.Value, GetUserToken());
                    result = Content(await _tokenService.CreateClientCredential(
                        settings,
                        _settings.Value.AuthorizationDomainId.Value,
                        new AuthorizationAPI.Models.ClientCredential
                        {
                            ClientId = clientCredential.ClientId,
                            Secret = clientCredential.Secret
                        }),
                        "text/plain");
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
            finally
            {
                await WriteMetrics("create-client-credential-token", start, result);
            }
            return result;
        }
    }
}
