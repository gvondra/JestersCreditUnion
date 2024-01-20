using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Loan.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI.Controllers
{
    [Route("api/LoanApplication/{id}/Rating")]
    [ApiController]
    public class LoanApplicationRatingController : LoanApiControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;
        private readonly ILoanApplicationRaterFactory _loanApplicationRaterFactory;
        private readonly IRatingSaver _ratingSaver;

        public LoanApplicationRatingController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanApplicationFactory loanApplicationFactory,
            ILoanApplicationRaterFactory loanApplicationRaterFactory,
            IRatingSaver ratingSaver)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
            _loanApplicationRaterFactory = loanApplicationRaterFactory;
            _ratingSaver = ratingSaver;
        }

        [HttpGet]
        [Authorize(Constants.POLICY_LOAN_APPLICATION_READ)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication innerLoanApplication = null;
                if (id.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing id parameter value");
                }
                else
                {
                    innerLoanApplication = await _loanApplicationFactory.Get(settings, id);
                    if (innerLoanApplication == null)
                        result = NotFound();
                }
                if (result != null && innerLoanApplication != null)
                {
                    IRating innerRating = await innerLoanApplication.GetRating(settings);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(mapper.Map<Rating>(innerRating));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-loan-application-rating", start, result);
            }
            return result;
        }

        [HttpPost]
        [Authorize(Constants.POLICY_LOAN_APPLICATION_READ)]
        public async Task<IActionResult> Create([FromRoute] Guid id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication innerLoanApplication = null;
                if (id.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing id parameter value");
                }
                else
                {
                    innerLoanApplication = await _loanApplicationFactory.Get(settings, id);
                    if (innerLoanApplication == null)
                        result = NotFound();
                }
                if (result != null && innerLoanApplication != null)
                {
                    ILoanApplicationRater rater = await _loanApplicationRaterFactory.Create();
                    IRating innerRating = rater.Rate(innerLoanApplication);
                    await _ratingSaver.SaveLoanApplicationRating(settings, innerLoanApplication.LoanApplicationId, innerRating);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(mapper.Map<Rating>(innerRating));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("create-loan-application-rating", start, result);
            }
            return result;
        }
    }
}
