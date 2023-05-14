using Autofac;
using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Framework;
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
    public class LoanController : APIControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;
        private readonly ILoanFactory _loanFactory;

        public LoanController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanApplicationFactory loanApplicationFactory,
            ILoanFactory loanFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
            _loanFactory = loanFactory;
        }

        [Authorize(Constants.POLICY_LOAN_CREATE)]
        [ProducesResponseType(typeof(Loan), 200)]
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Loan loan)
        {            
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ILoanApplication loanApplication = null;
                CoreSettings settings = GetCoreSettings();
                result = ValidateLoan(loan) ?? ValidateLoanAgreement(loan.Agreement);
                if (result == null && !loan.LoanApplicationId.HasValue)
                    result = BadRequest("Missing loan application id");
                if (result == null)
                {
                    loanApplication = await _loanApplicationFactory.Get(settings, loan.LoanApplicationId.Value);
                    if (loanApplication == null)
                        result = BadRequest("Loan application not found");
                }
                if (result == null && loanApplication != null)
                {
                    ILoan innerLoan = _loanFactory.Create(loanApplication);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    mapper.Map(loan, innerLoan);
                    mapper.Map(loan.Agreement, innerLoan.Agreement);
                    await innerLoan.Create(settings);
                    result = Ok(mapper.Map<Loan>(innerLoan));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("create-loan", start, result);
            }
            return result;
        }

        [NonAction]
        private IActionResult ValidateLoan(Loan loan)
        {
            IActionResult result = null;
            if (loan == null)
                result = BadRequest("Missing loan body data");
            return result;
        }

        [NonAction]
        private IActionResult ValidateLoanAgreement(LoanAgreement loanAgreement)
        {
            IActionResult result = null;
            if (loanAgreement == null)
                result = BadRequest("Missing loan agreement data");
            else if (string.IsNullOrEmpty(loanAgreement.BorrowerName))
                result = BadRequest("Missing borrower name");
            else if (!loanAgreement.BorrowerBirthDate.HasValue)
                result = BadRequest("Missing borrower birth date");
            else if (!string.IsNullOrEmpty(loanAgreement.CoBorrowerName) && !loanAgreement.CoBorrowerBirthDate.HasValue)
                result = BadRequest("Missing coborrower birth date");
            else if (!loanAgreement.OriginalAmount.HasValue)
                result = BadRequest("Missing original loan amount");
            else if (!loanAgreement.OriginalTerm.HasValue)
                result = BadRequest("Missing original term amount");
            else if (!loanAgreement.InterestRate.HasValue)
                result = BadRequest("Missing interest rate value");
            else if (loanAgreement.InterestRate.Value < 0.0M)
                result = BadRequest($"Invalid interest rate ({loanAgreement.InterestRate.Value}) less than zero");
            else if (1.0M <= loanAgreement.InterestRate.Value)
                result = BadRequest($"Invalid interest rate ({loanAgreement.InterestRate.Value}) greater than or equal to 1.0");
            else if (!loanAgreement.PaymentAmount.HasValue)
                result = BadRequest("Missing payment amount");
            else if (!loanAgreement.PaymentFrequency.HasValue)
                result = BadRequest("Missing payment frequency");
            return result;
        }
    }
}
