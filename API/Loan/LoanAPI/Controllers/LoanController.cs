using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Interface.Loan.Models;
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

namespace LoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : LoanApiControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;
        private readonly ILoanDisburser _loanDisburser;
        private readonly ILoanFactory _loanFactory;
        private readonly ILoanSaver _loanSaver;
        private readonly IAddressFactory _addressFactory;

        public LoanController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanController> logger,
            ILoanApplicationFactory loanApplicationFactory,
            ILoanDisburser loanDisburser,
            ILoanFactory loanFactory,
            ILoanSaver loanSaver,
            IAddressFactory addressFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
            _loanDisburser = loanDisburser;
            _loanFactory = loanFactory;
            _loanSaver = loanSaver;
            _addressFactory = addressFactory;
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [ProducesResponseType(typeof(Loan), 200)]
        [ProducesResponseType(typeof(Loan[]), 200)]
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] Guid? loanApplicationId,
            [FromQuery] string number,
            [FromQuery] string borrowerName,
            [FromQuery] DateTime? borrowerBirthDate)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ILoan innerLoan = null;
                IEnumerable<ILoan> innerLoans = null;
                IMapper mapper = MapperConfiguration.CreateMapper();
                CoreSettings settings = GetCoreSettings();
                if (loanApplicationId.HasValue && !loanApplicationId.Value.Equals(Guid.Empty))
                {
                    innerLoan = await _loanFactory.GetByLoanApplicationId(settings, loanApplicationId.Value);
                }
                else if (!string.IsNullOrEmpty(number))
                {
                    innerLoan = await _loanFactory.GetByNumber(settings, number);
                }
                else if (!string.IsNullOrEmpty(borrowerName) || borrowerBirthDate.HasValue)
                {
                    if (string.IsNullOrEmpty(borrowerName) || !borrowerBirthDate.HasValue)
                    {
                        result = BadRequest("Borrower name and birth date parameters must be used together.");
                    }
                    else
                    {
                        innerLoans = await _loanFactory.GetByNameBirthDate(settings, borrowerName, borrowerBirthDate.Value.Date);
                    }
                }
                if (result == null && innerLoan != null)
                {
                    result = Ok(
                        await Map(mapper, settings, innerLoan));
                }
                if (result == null && innerLoans != null)
                {
                    result = Ok(
                        await Task.WhenAll(
                            innerLoans.Select<ILoan, Task<Loan>>(ln => Map(mapper, settings, ln))));
                }
                else if (result == null)
                {
                    result = Ok(null);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("search-loan", start, result);
            }
            return result;
        }

        [Authorize(Constants.POLICY_LOAN_READ)]
        [ProducesResponseType(typeof(Loan), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoan innerLoan = null;
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                    result = BadRequest("Missing loan id parameter value");
                else
                    innerLoan = await _loanFactory.Get(settings, id.Value);
                if (result == null && innerLoan == null)
                {
                    result = NotFound();
                }
                else if (result == null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        await Map(mapper, settings, innerLoan));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-loan", start, result);
            }
            return result;
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
                {
                    result = BadRequest("Missing loan application id");
                }
                else if (result == null)
                {
                    loanApplication = await _loanApplicationFactory.Get(settings, loan.LoanApplicationId.Value);
                    if (loanApplication == null)
                        result = BadRequest("Loan application not found");
                }
                if (result == null && loanApplication != null)
                {
                    ILoan innerLoan = _loanFactory.Create(loanApplication);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    MapAgreement(mapper, loan, innerLoan);
                    await _loanSaver.Create(settings, innerLoan);
                    result = Ok(
                        await Map(mapper, settings, innerLoan));
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

        [Authorize(Constants.POLICY_LOAN_EDIT)]
        [ProducesResponseType(typeof(Loan), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid? id, [FromBody] Loan loan)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ILoan innerLoan = null;
                CoreSettings settings = GetCoreSettings();
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                {
                    result = BadRequest("Missing id parameter value");
                }
                if (result == null)
                {
                    result = ValidateLoan(loan) ?? ValidateLoanAgreement(loan.Agreement);
                }
                if (result == null)
                {
                    innerLoan = await _loanFactory.Get(settings, id.Value);
                    if (innerLoan == null)
                        result = NotFound();
                }
                if (result == null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    MapAgreement(mapper, loan, innerLoan);
                    await _loanSaver.Update(settings, innerLoan);
                    result = Ok(
                        await Map(mapper, settings, innerLoan));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("update-loan", start, result);
            }
            return result;
        }

        [NonAction]
        private void MapAgreement(IMapper mapper, Loan loan, ILoan innerLoan)
        {
            mapper.Map(loan, innerLoan);
            mapper.Map(loan.Agreement, innerLoan.Agreement);
            innerLoan.Agreement.SetBorrowerAddress(
                GetAddress(loan.Agreement.BorrowerAddress));
            innerLoan.Agreement.SetCoBorrowerAddress(
                GetAddress(loan.Agreement.CoBorrowerAddress));
        }

        [NonAction]
        private IAddress GetAddress(Address address)
        {
            IAddress innerAddress = null;
            if (address != null)
            {
                string state = address.State;
                string postalCode = address.PostalCode;
                innerAddress = _addressFactory.Create(address.Recipient, address.Attention, address.Delivery, address.Secondary, address.City, ref state, ref postalCode);
            }
            return innerAddress;
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

        [NonAction]
        private static async Task<Loan> Map(IMapper mapper, CoreSettings settings, ILoan innerLoan)
        {
            IAddress borrowerAddress = await innerLoan.Agreement.GetBorrowerAddress(settings);
            IAddress coborrowerAddress = await innerLoan.Agreement.GetCoBorrowerAddress(settings);

            Loan loan = mapper.Map<Loan>(innerLoan);
            loan.Agreement.BorrowerAddress = borrowerAddress != null ? mapper.Map<Address>(borrowerAddress) : null;
            loan.Agreement.CoBorrowerAddress = coborrowerAddress != null ? mapper.Map<Address>(coborrowerAddress) : null;
            loan.StatusDescription = await innerLoan.GetStatusDescription(settings);

            return loan;
        }

        [Authorize(Constants.POLICY_LOAN_EDIT)]
        [ProducesResponseType(typeof(Loan), 200)]
        [HttpPost("{id}/InitiateDisbursement")]
        public async Task<IActionResult> InitiateDisbursement([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoan innerLoan = null;
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing loan id parameter value");
                }
                else
                {
                    innerLoan = await _loanFactory.Get(settings, id.Value);
                    if (innerLoan == null)
                        result = NotFound();
                }
                if (result == null)
                {
                    if (innerLoan.Agreement.Status == JestersCreditUnion.Loan.Framework.Enumerations.LoanAgrementStatus.PendingSignoff)
                        innerLoan.Agreement.Status = JestersCreditUnion.Loan.Framework.Enumerations.LoanAgrementStatus.Agreed;
                    await _loanSaver.InitiateDisburseFundsUpdate(settings, innerLoan);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        await Map(mapper, settings, innerLoan));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("initiate-disburse-loan", start, result);
            }
            return result;
        }

        [Authorize(Constants.POLICY_LOAN_EDIT)]
        [ProducesResponseType(typeof(Loan), 200)]
        [HttpPost("{id}/Disbursement")]
        public async Task<IActionResult> Disbursement([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoan innerLoan = null;
                if (!id.HasValue || id.Value.Equals(Guid.Empty))
                {
                    result = BadRequest("Missing loan id parameter value");
                }
                else
                {
                    innerLoan = await _loanFactory.Get(settings, id.Value);
                    if (innerLoan == null)
                        result = NotFound();
                }
                if (result == null)
                {
                    ITransaction transaction = await _loanDisburser.Disburse(settings, innerLoan);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    DisburseResponse disburseResponse = new DisburseResponse
                    {
                        Loan = await Map(mapper, settings, innerLoan),
                        Transaction = mapper.Map<Transaction>(transaction)
                    };
                    result = Ok(disburseResponse);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("disburse-loan", start, result);
            }
            return result;
        }
    }
}
