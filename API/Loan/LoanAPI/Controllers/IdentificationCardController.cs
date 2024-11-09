using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Loan.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI.Controllers
{
    [Route("api/LoanApplication/{id}")]
    [ApiController]
    public class IdentificationCardController : LoanApiControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;

        public IdentificationCardController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<IdentificationCardController> logger,
            ILoanApplicationFactory loanApplicationFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [HttpGet("BorrowerIdentificationCard")]
        public async Task<IActionResult> GetBorrowerIdentificationCard([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication loanApplication = null;
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing loan application id parameter value");
                }
                else
                {
                    loanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (loanApplication == null)
                        result = NotFound();
                }
                if (result == null && loanApplication != null)
                {
                    IIdentificationCardReader reader = loanApplication.CreateIdentificationCardReader();
                    Stream stream = await reader.ReadBorrowerIdentificationCard(settings);
                    result = File(stream, "application/octet-stream");
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-borrower-id-card", start, result);
            }
            return result;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [HttpPost("BorrowerIdentificationCard")]
        public async Task<IActionResult> SaveBorrowerIdentificationCard([FromRoute] Guid? id)
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
                    if (formCollection.Files.Count == 0)
                    {
                        result = BadRequest("No file received");
                    }
                }
                if (result == null && id.HasValue)
                {
                    loanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (loanApplication == null)
                        result = NotFound();
                }
                if (result == null && formCollection != null && loanApplication != null)
                {
                    IFormFile formFile = formCollection.Files[0];
                    using (Stream stream = formFile.OpenReadStream())
                    {
                        IIdentificationCardSaver saver = loanApplication.CreateIdentificationCardSaver();
                        await saver.SaveBorrowerIdentificationCard(settings, stream);
                    }
                    result = Ok();
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
