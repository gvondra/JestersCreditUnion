using AutoMapper;
using BrassLoon.Interface.WorkTask.Models;
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
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

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
        private readonly WorkTaskAPI.IWorkTaskService _workTaskService;

        public LoanApplicationController(IOptions<Settings> settings,
            ISettingsFactory settingsFactory,
            AuthorizationAPI.IUserService userService,
            ILogger<LoanApplicationController> logger,
            ILoanApplicationFactory loanApplicationFactory,
            ILoanApplicationSaver loanApplicationSaver,
            IAddressFactory addressFactory,
            IEmailAddressFactory emailAddressFactory,
            IPhoneFactory phoneFactory,
            WorkTaskAPI.IWorkTaskService workTaskService)
            : base(settings, settingsFactory, userService, logger)
        {
            _loanApplicationFactory = loanApplicationFactory;
            _loanApplicationSaver = loanApplicationSaver;
            _addressFactory = addressFactory;
            _emailAddressFactory = emailAddressFactory;
            _phoneFactory = phoneFactory;
            _workTaskService = workTaskService;
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

                    loanApplication = mapper.Map<LoanApplication>(innerLoanApplication);
                    loanApplication.BorrowerAddress = borrowerAddress != null ? mapper.Map<Address>(borrowerAddress) : null;
                    loanApplication.CoBorrowerAddress = coborrowerAddress != null ? mapper.Map<Address>(coborrowerAddress) : null;
                    loanApplication.BorrowerEmailAddress = borrowerEmailAddress != null ? borrowerEmailAddress.Address : string.Empty;
                    loanApplication.CoBorrowerEmailAddress = coborrowerEmailAddress != null ? coborrowerEmailAddress.Address : string.Empty;
                    loanApplication.BorrowerPhone = borrowerPhone != null ? borrowerPhone.Number : string.Empty;
                    loanApplication.CoBorrowerPhone = coborrowerPhone != null ? coborrowerPhone.Number : string.Empty;

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
    }
}
