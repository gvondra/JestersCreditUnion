using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressInterface = BrassLoon.Interface.Address;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplicationFactory : ILoanApplicationFactory
    {
        private readonly ILoanApplicationDataFactory _dataFactory;
        private readonly ILoanApplicationDataSaver _dataSaver;
        private readonly ILookupFactory _lookupFactory;
        private readonly IIdentificationCardDataSaver _identificationCardDataSaver;
        private readonly AddressInterface.IPhoneService _phoneService;
        private readonly AddressInterface.IEmailAddressService _emailService;
        private readonly SettingsFactory _settingsFactory;

        public LoanApplicationFactory(
            ILoanApplicationDataFactory dataFactory,
            ILoanApplicationDataSaver dataSaver,
            ILookupFactory lookupFactory,
            IIdentificationCardDataSaver identificationCardDataSaver,
            AddressInterface.IPhoneService phoneService,
            AddressInterface.IEmailAddressService emailService,
            SettingsFactory settingsFactory)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _lookupFactory = lookupFactory;
            _identificationCardDataSaver = identificationCardDataSaver;
            _phoneService = phoneService;
            _emailService = emailService;
            _settingsFactory = settingsFactory;
        }

        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }

        private LoanApplication Create(LoanApplicationData data)
        {
            return new LoanApplication(
                data,
                _dataSaver,
                this,
                _lookupFactory,
                _identificationCardDataSaver,
                _phoneService,
                _emailService,
                _settingsFactory);
        }

        private async Task<LoanApplication> Create(ISettings settings, LoanApplicationData data)
        {
            LoanApplication loanApplication = Create(data);
            AddressInterface.ISettings addressSettings = _settingsFactory.CreateAddress(settings);
            AddressInterface.Models.Phone phone;
            AddressInterface.Models.EmailAddress email;
            if (loanApplication.BorrowerPhoneId.HasValue)
            {
                phone = await _phoneService.Get(addressSettings, settings.AddressDomainId.Value, loanApplication.BorrowerPhoneId.Value);
                loanApplication.BorrowerPhone = phone?.Number ?? string.Empty;
            }
            if (loanApplication.CoBorrowerPhoneId.HasValue)
            {
                phone = await _phoneService.Get(addressSettings, settings.AddressDomainId.Value, loanApplication.CoBorrowerPhoneId.Value);
                loanApplication.CoBorrowerPhone = phone?.Number ?? string.Empty;
            }
            if (loanApplication.BorrowerEmailAddressId.HasValue)
            {
                email = await _emailService.Get(addressSettings, settings.AddressDomainId.Value, loanApplication.BorrowerEmailAddressId.Value);
                loanApplication.BorrowerEmailAddress = email?.Address ?? string.Empty;
            }
            if (loanApplication.CoBorrowerEmailAddressId.HasValue)
            {
                email = await _emailService.Get(addressSettings, settings.AddressDomainId.Value, loanApplication.CoBorrowerEmailAddressId.Value);
                loanApplication.CoBorrowerEmailAddress = email?.Address ?? string.Empty;
            }
            return loanApplication;
        }

        public ILoanApplication Create(Guid userId)
        {
            return Create(
                new LoanApplicationData
                {
                    LoanApplicationId = Guid.NewGuid(),
                    UserId = userId,
                    ApplicationDate = DateTime.Today
                });
        }

        public async Task<ILoanApplication> Get(ISettings settings, Guid id)
        {
            LoanApplicationData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            return data != null ? await Create(settings, data) : null;
        }

        public async Task<IEnumerable<ILoanApplication>> GetByUserId(ISettings settings, Guid userId)
        {
            IEnumerable<LoanApplicationData> data = await _dataFactory.GetByUserId(new CommonCore.DataSettings(settings), userId);
            return await Task.WhenAll(data.Select(d => Create(settings, d)));
        }
    }
}
