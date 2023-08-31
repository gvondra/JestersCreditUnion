using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Framework;
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
    [Route("api/LoanApplication/{id}")]
    [ApiController]
    public class IdentificationCardController : APIControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;

        public IdentificationCardController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<IdentificationCardController> logger,
            ILoanApplicationFactory loanApplicationFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [HttpPost("BorrowerIdentificationCard")]
        public async Task<IActionResult> BorrowerIdentificationCard([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication loanApplication = null;
                IFormCollection formCollection = null;
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing loan application id parameter value");
                }
                else
                {
                    formCollection = await Request.ReadFormAsync();
                    if (formCollection.Count == 0)
                    {
                        result = BadRequest("No file received");
                    }
                }
                if (result == null)
                {
                    loanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (loanApplication == null)
                        result = NotFound();
                }
                if (result == null && formCollection != null && loanApplication != null)
                {
                    IFormFile formFile = formCollection.Files[0];
                    IIdentificationCardSaver saver = loanApplication.CreateIdentificationCardSaver();
                    await saver.SaveBorrowerIdentificationCard(settings);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("add-borrower-id-card", start, result);
            }
            return result;
        }
    }
}
