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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : APIControllerBase
    {
        private readonly AuthorizationAPI.IRoleService _roleService;

        public RoleController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<RoleController> logger,
            AuthorizationAPI.IRoleService roleService)
            : base(settings, settingsFactory, userService, logger)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_ROLE_EDIT)]
        [ProducesResponseType(typeof(List<Role>), 200)]
        public async Task<IActionResult> Search()
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                AuthorizationAPI.ISettings settings = GetAuthorizationSettings();
                IMapper mapper = MapperConfiguration.CreateMapper();
                result = Ok(
                    (await _roleService.GetByDomainId(settings, _settings.Value.AuthorizationDomainId.Value))
                    .Select<AuthorizationAPI.Models.Role, Role>(mapper.Map<Role>));
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("search-roles", start, result);
            }
            return result;
        }

        [NonAction]
        private IActionResult ValidateCreate(Role role)
        {
            IActionResult result = Validate(role);
            if (result == null && string.IsNullOrEmpty(role.PolicyName))
                result = BadRequest("Missing role policy name value");
            return result;
        }

        [NonAction]
        private IActionResult Validate(Role role)
        {
            IActionResult result = null;
            if (role == null)
                result = BadRequest("Missing role data body");
            else if (string.IsNullOrEmpty(role.Name))
                result = BadRequest("Missing role name value");
            return result;
        }

        [HttpPost]
        [Authorize(Constants.POLICY_ROLE_EDIT)]
        [ProducesResponseType(typeof(Role), 200)]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                result = ValidateCreate(role);
                if (result == null)
                {
                    AuthorizationAPI.ISettings settings = GetAuthorizationSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    AuthorizationAPI.Models.Role innerRole = mapper.Map<AuthorizationAPI.Models.Role>(role);
                    innerRole = await _roleService.Create(settings, _settings.Value.AuthorizationDomainId.Value, innerRole);
                    result = Ok(
                        mapper.Map<Role>(innerRole));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("create-role", start, result);
            }
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Constants.POLICY_ROLE_EDIT)]
        [ProducesResponseType(typeof(Role), 200)]
        public async Task<IActionResult> Update([FromRoute] Guid? id, [FromBody] Role role)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                    result = BadRequest("Missing or invalid role id parameter value");
                else
                    result = Validate(role);
                if (result == null && id.HasValue)
                {
                    AuthorizationAPI.ISettings settings = GetAuthorizationSettings();
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    AuthorizationAPI.Models.Role innerRole = mapper.Map<AuthorizationAPI.Models.Role>(role);
                    innerRole = await _roleService.Update(settings, _settings.Value.AuthorizationDomainId.Value, id.Value, innerRole);
                    result = Ok(
                        mapper.Map<Role>(innerRole));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("update-role", start, result, new Dictionary<string, string> { { nameof(id), id?.ToString() ?? string.Empty } });
            }
            return result;
        }
    }
}
