using AutoMapper;
using JestersCreditUnion.CommonAPI;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
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
using Autofac.Features.Indexed;

namespace LoanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : LoanApiControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;
        private readonly ILoanApplicationSaver _loanApplicationSaver;
        private readonly IAddressFactory _addressFactory;
        private readonly IAddressSaver _addressSaver;
        private readonly AuthorizationAPI.IUserService _userService;

        public LoanApplicationController(
            IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanApplicationFactory loanApplicationFactory,
            ILoanApplicationSaver loanApplicationSaver,
            IIndex<string, IAddressFactory> addressFactoryIndex,
            IAddressSaver addressSaver)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
            _loanApplicationSaver = loanApplicationSaver;
            _addressFactory = addressFactoryIndex["v2"];
            _addressSaver = addressSaver;
            _userService = userService;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(LoanApplication[]), 200)]
        [HttpGet()]
        public async Task<IActionResult> Search([FromQuery] bool byRequestor = false, [FromQuery] Guid? userId = null)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                if (userId.HasValue && userId.Value.Equals(Guid.Empty))
                {
                    userId = null;
                }
                if (!userId.HasValue && byRequestor)
                {
                    userId = await GetCurrentUserId();
                }
                IEnumerable<ILoanApplication> innerLoanApplications = new List<ILoanApplication>();
                CoreSettings settings = GetCoreSettings();
                if (result == null && userId.HasValue)
                {
                    innerLoanApplications = await _loanApplicationFactory.GetByUserId(settings, userId.Value);
                }
                else if (result == null)
                {
                    result = BadRequest("No filter parameter specified");
                }
                if (result == null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    List<LoanApplication> loanApplications = new List<LoanApplication>();
                    foreach (ILoanApplication innerLoanApplication in innerLoanApplications)
                    {
                        if (await CanAccessLoanApplication(innerLoanApplication))
                            loanApplications.Add(await Map(mapper, settings, innerLoanApplication));
                    }
                    result = Ok(loanApplications);
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("search-loan-application", start, result);
            }
            return result;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(LoanApplication), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid? id)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication innerLoanApplication = null;
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null)
                {
                    innerLoanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (innerLoanApplication == null || !await CanAccessLoanApplication(innerLoanApplication))
                        result = NotFound();
                }
                if (result == null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(await Map(mapper, settings, innerLoanApplication));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("get-loan-application", start, result);
            }
            return result;
        }

        [NonAction]
        private async Task<bool> CanAccessLoanApplication(ILoanApplication loanApplication)
        {
            bool canAccess = UserHasRole(Constants.POLICY_LOAN_APPLICATION_EDIT)
                || UserHasRole(Constants.POLICY_LOAN_APPLICATION_READ);
            if (!canAccess)
            {
                Guid? userId = await GetCurrentUserId();
                if (userId.HasValue)
                    canAccess = userId.Value.Equals(loanApplication.UserId);
            }
            return canAccess;
        }

        [NonAction]
        private async Task<bool> CanAccessLoanApplicationComment(ILoanApplicationComment comment)
        {
            bool canAccess = !comment.IsInternal
                || UserHasRole(Constants.POLICY_LOAN_APPLICATION_EDIT)
                || UserHasRole(Constants.POLICY_LOAN_APPLICATION_READ);
            if (!canAccess)
            {
                Guid? userId = await GetCurrentUserId();
                if (userId.HasValue)
                    canAccess = userId.Value.Equals(comment.UserId);
            }
            return canAccess;
        }

        [NonAction]
        private bool IsInternalComment()
        {
            bool isInternal = UserHasRole(Constants.POLICY_LOAN_APPLICATION_EDIT)
                || UserHasRole(Constants.POLICY_LOAN_APPLICATION_READ);
            return isInternal;
        }

        [NonAction]
        private void Map(IMapper mapper, LoanApplication loanApplication, ILoanApplication innerLoanApplication)
        {
            mapper.Map(loanApplication, innerLoanApplication);
            innerLoanApplication.SetBorrowerAddress(
                GetAddress(loanApplication.BorrowerAddress));
            innerLoanApplication.SetCoBorrowerAddress(
                GetAddress(loanApplication.CoBorrowerAddress));
        }

        [NonAction]
        private async Task<LoanApplication> Map(IMapper mapper, CoreSettings settings, ILoanApplication innerLoanApplication)
        {
            IAddress borrowerAddress = await innerLoanApplication.GetBorrowerAddress(settings);
            IAddress coborrowerAddress = await innerLoanApplication.GetCoBorrowerAddress(settings);

            LoanApplication loanApplication = mapper.Map<LoanApplication>(innerLoanApplication);
            loanApplication.BorrowerAddress = borrowerAddress != null ? mapper.Map<Address>(borrowerAddress) : null;
            loanApplication.CoBorrowerAddress = coborrowerAddress != null ? mapper.Map<Address>(coborrowerAddress) : null;
            loanApplication.StatusDescription = await innerLoanApplication.GetStatusDescription(settings);

            await MapComments(mapper, innerLoanApplication, loanApplication);

            if (loanApplication.Denial != null && loanApplication.Denial.UserId.HasValue)
                loanApplication.Denial.UserName = await _userService.GetName(GetAuthorizationSettings(), _settings.Value.AuthorizationDomainId.Value, loanApplication.Denial.UserId.Value);
            if (loanApplication.Denial != null)
            {
                loanApplication.Denial.ReasonDescription = await innerLoanApplication.Denial.GetReasonDescription(settings);
            }

            return loanApplication;
        }

        [NonAction]
        private async Task MapComments(IMapper mapper, ILoanApplication innerLoanApplication, LoanApplication loanApplication)
        {
            if (innerLoanApplication.Comments != null)
            {
                AuthorizationAPI.ISettings settings = GetAuthorizationSettings();
                loanApplication.Comments = new List<LoanApplicationComment>();
                foreach (ILoanApplicationComment innerComment in innerLoanApplication.Comments.OrderByDescending(c => c.CreateTimestamp))
                {
                    if (await CanAccessLoanApplicationComment(innerComment))
                    {
                        loanApplication.Comments.Add(
                            await MapComment(mapper, settings, innerComment));
                    }
                }
            }
        }

        [NonAction]
        private async Task<LoanApplicationComment> MapComment(IMapper mapper, AuthorizationAPI.ISettings settings, ILoanApplicationComment innerComment)
        {
            LoanApplicationComment comment = mapper.Map<LoanApplicationComment>(innerComment);
            comment.UserName = await _userService.GetName(settings, _settings.Value.AuthorizationDomainId.Value, innerComment.UserId);
            return comment;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] LoanApplication loanApplication)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication innerLoanApplication = _loanApplicationFactory.Create((await GetCurrentUserId()).Value);
                IMapper mapper = MapperConfiguration.CreateMapper();
                Map(mapper, loanApplication, innerLoanApplication);

                await _loanApplicationSaver.Create(settings, innerLoanApplication);

                loanApplication = await Map(mapper, settings, innerLoanApplication);

                result = Ok(loanApplication);
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("create-loan-application", start, result);
            }
            return result;
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

        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(typeof(LoanApplicationComment), 200)]
        [HttpPost("{id}/Comment")]
        public async Task<IActionResult> AppendComment([FromRoute] Guid? id, [FromBody] LoanApplicationComment comment, [FromQuery] bool isPublic = false)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                ILoanApplication innerLoanApplication = null;
                CoreSettings settings = GetCoreSettings();
                if (result == null && string.IsNullOrEmpty(comment?.Text))
                    result = Ok();
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id paramter value");
                if (result == null)
                {
                    innerLoanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (!await CanAccessLoanApplication(innerLoanApplication))
                        result = NotFound();
                }
                if (result == null && innerLoanApplication != null)
                {
                    Guid? userId = await GetCurrentUserId();
                    bool isInternal = !isPublic;
                    if (!isPublic)
                        isInternal = IsInternalComment();
                    ILoanApplicationComment innerComment = innerLoanApplication.CreateComment(comment.Text, userId.Value, isInternal);
                    await _loanApplicationSaver.CreateComment(settings, innerComment);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    result = Ok(
                        await MapComment(mapper, GetAuthorizationSettings(), innerComment));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("append-loan-application-comment", start, result);
            }
            return result;
        }

        [Authorize(Constants.POLICY_LOAN_APPLICATION_EDIT)]
        [ProducesResponseType(typeof(LoanApplication), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid? id, [FromBody] LoanApplication loanApplication)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication innerLoanApplication = null;
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null && loanApplication == null)
                    result = BadRequest("Missing loan application details");
                if (result == null)
                {
                    innerLoanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (innerLoanApplication == null || !await CanAccessLoanApplication(innerLoanApplication))
                        result = NotFound();
                }
                if (result == null && innerLoanApplication != null)
                {
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    Map(mapper, loanApplication, innerLoanApplication);
                    await _loanApplicationSaver.Update(settings, innerLoanApplication);
                    result = Ok(await Map(mapper, settings, innerLoanApplication));
                }
            }
            catch (System.Exception ex)
            {
                WriteException(ex);
                result = StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                await WriteMetrics("update-loan-application", start, result);
            }
            return result;
        }

        [Authorize(Constants.POLICY_LOAN_APPLICATION_EDIT)]
        [ProducesResponseType(200)]
        [HttpPut("{id}/Denial")]
        public async Task<IActionResult> Deny([FromRoute] Guid? id, [FromBody] LoanApplicationDenial denial)
        {
            DateTime start = DateTime.UtcNow;
            IActionResult result = null;
            try
            {
                CoreSettings settings = GetCoreSettings();
                ILoanApplication innerLoanApplication = null;
                if (result == null && (!id.HasValue || id.Value.Equals(Guid.Empty)))
                    result = BadRequest("Missing id parameter value");
                if (result == null && denial == null)
                    result = BadRequest("Missing loan application denail details");
                if (result == null && string.IsNullOrEmpty(denial.Text))
                    result = BadRequest("Missing denial text");
                if (result == null)
                {
                    innerLoanApplication = await _loanApplicationFactory.Get(settings, id.Value);
                    if (innerLoanApplication == null)
                        result = NotFound();
                }
                if (result == null && innerLoanApplication != null)
                {
                    innerLoanApplication.SetDenial((LoanApplicationDenialReason)denial.Reason, denial.Date ?? DateTime.Today, (await GetCurrentUserId()).Value, denial.Text);
                    await _loanApplicationSaver.Deny(settings, innerLoanApplication);
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
                await WriteMetrics("deny-loan-application", start, result);
            }
            return result;
        }
    }
}
