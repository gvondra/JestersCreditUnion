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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : APIControllerBase
    {
        private readonly ILoanApplicationFactory _loanApplicationFactory;
        private readonly ILoanApplicationSaver _loanApplicationSaver;
        private readonly IAddressFactory _addressFactory;
        private readonly IEmailAddressFactory _emailAddressFactory;
        private readonly IPhoneFactory _phoneFactory; 
        private readonly AuthorizationAPI.IUserService _userService;

        public LoanApplicationController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanApplicationFactory loanApplicationFactory,
            ILoanApplicationSaver loanApplicationSaver,
            IAddressFactory addressFactory,
            IEmailAddressFactory emailAddressFactory,
            IPhoneFactory phoneFactory)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
            _loanApplicationSaver = loanApplicationSaver;
            _addressFactory = addressFactory;
            _emailAddressFactory = emailAddressFactory;
            _phoneFactory = phoneFactory;
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
                    innerLoanApplications = await _loanApplicationFactory.GetAll(settings);
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
                    if (innerLoanApplication == null || !(await CanAccessLoanApplication(innerLoanApplication)))
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
        private async Task<LoanApplication> Map(IMapper mapper, CoreSettings settings, ILoanApplication innerLoanApplication)
        {
            IAddress borrowerAddress = await innerLoanApplication.GetBorrowerAddress(settings);
            IAddress coborrowerAddress = await innerLoanApplication.GetCoBorrowerAddress(settings);
            IEmailAddress borrowerEmailAddress = await innerLoanApplication.GetBorrowerEmailAddress(settings);
            IEmailAddress coborrowerEmailAddress = await innerLoanApplication.GetCoBorrowerEmailAddress(settings);
            IPhone borrowerPhone = await innerLoanApplication.GetBorrowerPhone(settings);
            IPhone coborrowerPhone = await innerLoanApplication.GetCoBorrowerPhone(settings);

            LoanApplication loanApplication = mapper.Map<LoanApplication>(innerLoanApplication);
            loanApplication.BorrowerAddress = borrowerAddress != null ? mapper.Map<Address>(borrowerAddress) : null;
            loanApplication.CoBorrowerAddress = coborrowerAddress != null ? mapper.Map<Address>(coborrowerAddress) : null;
            loanApplication.BorrowerEmailAddress = borrowerEmailAddress != null ? borrowerEmailAddress.Address : string.Empty;
            loanApplication.CoBorrowerEmailAddress = coborrowerEmailAddress != null ? coborrowerEmailAddress.Address : string.Empty;
            loanApplication.BorrowerPhone = borrowerPhone != null ? borrowerPhone.Number : string.Empty;
            loanApplication.CoBorrowerPhone = coborrowerPhone != null ? coborrowerPhone.Number : string.Empty;
            loanApplication.StatusDescription = await innerLoanApplication.GetStatusDescription(settings);
            
            await MapComments(mapper, innerLoanApplication, loanApplication);

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
                IAddress borrowerAddress = await GetAddress(settings, loanApplication.BorrowerAddress);
                IAddress coborrowerAddress = await GetAddress(settings, loanApplication.CoBorrowerAddress);
                IEmailAddress borrowerEmailAddress = await GetEmailAddress(settings, loanApplication.BorrowerEmailAddress);
                IEmailAddress coborrowerEmailAddress = await GetEmailAddress(settings, loanApplication.CoBorrowerEmailAddress);
                IPhone borrowerPhone = await GetPhone(settings, loanApplication.BorrowerPhone);
                IPhone coborrowerPhone = await GetPhone(settings, loanApplication.CoBorrowerPhone);
                if (result == null)
                {
                    ILoanApplication innerLoanApplication = _loanApplicationFactory.Create((await GetCurrentUserId()).Value);
                    IMapper mapper = MapperConfiguration.CreateMapper();
                    mapper.Map(loanApplication, innerLoanApplication);
                    innerLoanApplication.BorrowerAddressId = borrowerAddress?.AddressId;
                    innerLoanApplication.CoBorrowerAddressId = coborrowerAddress?.AddressId;
                    innerLoanApplication.BorrowerEmailAddressId = borrowerEmailAddress?.EmailAddressId;
                    innerLoanApplication.CoBorrowerEmailAddressId = coborrowerEmailAddress?.EmailAddressId;
                    innerLoanApplication.BorrowerPhoneId = borrowerPhone?.PhoneId;
                    innerLoanApplication.CoBorrowerPhoneId = coborrowerPhone?.PhoneId;

                    await _loanApplicationSaver.Create(settings, innerLoanApplication);

                    loanApplication = await Map(mapper, settings, innerLoanApplication);

                    result = Ok(loanApplication);
                }
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
        private async Task<IPhone> GetPhone(ISettings settings, string number)
        {
            IPhone innerPhone = null;
            if (!string.IsNullOrEmpty(number))
            {
                innerPhone = await _phoneFactory.Get(settings, number);
                if (innerPhone == null)
                {
                    innerPhone = _phoneFactory.Create(ref number);
                    await innerPhone.Create(settings);
                }   
            }
            return innerPhone;
        }

        [NonAction]
        private async Task<IEmailAddress> GetEmailAddress(ISettings settings, string address)
        {
            IEmailAddress innerEmailAddress = null;
            if (!string.IsNullOrEmpty(address))
            {
                innerEmailAddress = await _emailAddressFactory.Get(settings, address);
                if (innerEmailAddress == null)
                {
                    innerEmailAddress = _emailAddressFactory.Create(address);
                    await innerEmailAddress.Create(settings);
                }
                    
            }
            return innerEmailAddress;
        }

        [NonAction]
        private async Task<IAddress> GetAddress(ISettings settings, Address address)
        {
            IAddress innerAddress = null;
            if (address != null)
            {
                string state = address.State;
                string postalCode = address.PostalCode;
                innerAddress = _addressFactory.Create(address.Recipient, address.Attention, address.Delivery, address.Secondary, address.City, ref state, ref postalCode);

                IAddress existingAddress = await _addressFactory.GetByHash(settings, innerAddress.Hash);
                if (existingAddress != null)
                {
                    innerAddress = existingAddress;
                }
                else
                {
                    await innerAddress.Create(settings);
                }
            }
            return innerAddress;
        }

        [Authorize(Constants.POLICY_BL_AUTH)]
        [ProducesResponseType(200)]
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
                    if (!(await CanAccessLoanApplication(innerLoanApplication)))
                        result = NotFound();
                }
                if (result == null && innerLoanApplication != null)
                {
                    Guid? userId = await GetCurrentUserId();
                    bool isInternal = !isPublic;
                    if (!isPublic)
                        isInternal = IsInternalComment();
                    ILoanApplicationComment innerComment = innerLoanApplication.CreateComment(comment.Text, userId.Value, isInternal);
                    await innerComment.Create(settings);
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
    }
}
