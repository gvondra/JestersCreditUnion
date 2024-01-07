using Autofac.Features.Indexed;
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
    public class LoanFactory : ILoanFactory
    {
        private readonly ILoanDataFactory _dataFactory;
        private readonly ILoanDataSaver _dataSaver;
        private readonly LoanNumberGenerator _numberGenerator;
        private readonly IPaymentFactory _paymentFactory;
        private readonly ILookupFactory _lookupFactory;
        private readonly AddressInterface.IPhoneService _phoneService;
        private readonly AddressInterface.IEmailAddressService _emailService;
        private readonly SettingsFactory _settingsFactory;
        private readonly IAddressFactory _addressFactory;

        public LoanFactory(ILoanDataFactory dataFactory,
            ILoanDataSaver dataSaver,
            LoanNumberGenerator numberGenerator,
            IPaymentDataFactory paymentDataFactory,
            IPaymentDataSaver paymentDataSaver,
            ITransactionDataSaver transactionDataSaver,
            ILookupFactory lookupFactory,
            AddressInterface.IPhoneService phoneService,
            AddressInterface.IEmailAddressService emailService,
            SettingsFactory settingsFactory,
            IIndex<string, IAddressFactory> addressFactoryIndex)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _numberGenerator = numberGenerator;
            _paymentFactory = new PaymentFactory(paymentDataFactory, transactionDataSaver, paymentDataSaver);
            _lookupFactory = lookupFactory;
            _phoneService = phoneService;
            _emailService = emailService;
            _settingsFactory = settingsFactory;
            _addressFactory = addressFactoryIndex["v2"];

        }

        public IAddressFactory AddressFactory => _addressFactory;
        public ITransactionFacatory TransactionFacatory { get; set; }

        private Loan Create(LoanData data) => new Loan(data, _settingsFactory, _phoneService, _emailService, _dataSaver, this, _paymentFactory, _lookupFactory);

        private async Task<Loan> Create(ISettings settings, LoanData data)
        {
            Loan loan = Create(data);
            AddressInterface.ISettings addressSettings = _settingsFactory.CreateAddress(settings);
            AddressInterface.Models.Phone phone;
            AddressInterface.Models.EmailAddress email;
            if (data.Agreement?.BorrowerEmailAddressId.HasValue ?? false)
            {
                email = await _emailService.Get(addressSettings, settings.AddressDomainId.Value, data.Agreement.BorrowerEmailAddressId.Value);
                loan.Agreement.BorrowerEmailAddress = email?.Address ?? string.Empty;
            }
            if (data.Agreement?.CoBorrowerEmailAddressId.HasValue ?? false)
            {
                email = await _emailService.Get(addressSettings, settings.AddressDomainId.Value, data.Agreement.CoBorrowerEmailAddressId.Value);
                loan.Agreement.CoBorrowerEmailAddress = email?.Address ?? string.Empty;
            }
            if (data.Agreement?.BorrowerPhoneId.HasValue ?? false)
            {
                phone = await _phoneService.Get(addressSettings, settings.AddressDomainId.Value, data.Agreement.BorrowerPhoneId.Value);
                loan.Agreement.BorrowerPhone = phone.Number ?? string.Empty;
            }
            if (data.Agreement?.CoBorrowerPhoneId.HasValue ?? false)
            {
                phone = await _phoneService.Get(addressSettings, settings.AddressDomainId.Value, data.Agreement.CoBorrowerPhoneId.Value);
                loan.Agreement.CoBorrowerPhone = phone.Number ?? string.Empty;
            }
            return loan;
        }

        public ILoan Create(ILoanApplication loanApplication)
        {
            Guid loanId = Guid.NewGuid();
            return Create(
                new LoanData
                {
                    LoanId = loanId,
                    Number = _numberGenerator.Generate(),
                    LoanApplicationId = loanApplication.LoanApplicationId,
                    Agreement = new LoanAgreementData
                    {
                        LoanId = loanId,
                        CreateDate = DateTime.Today
                    }
                });
        }

        public async Task<ILoan> GetByNumber(ISettings settings, string number)
        {
            Loan result = null;
            LoanData data = await _dataFactory.GetByNumber(new CommonCore.DataSettings(settings), number);
            if (data != null)
                result = await Create(settings, data);
            return result;
        }

        public async Task<ILoan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId)
        {
            Loan result = null;
            LoanData data = await _dataFactory.GetByLoanApplicationId(new CommonCore.DataSettings(settings), loanApplicationId);
            if (data != null)
                result = await Create(settings, data);
            return result;
        }

        public async Task<ILoan> Get(ISettings settings, Guid id)
        {
            Loan result = null;
            LoanData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            if (data != null)
                result = await Create(settings, data);
            return result;
        }

        public async Task<IEnumerable<ILoan>> GetByNameBirthDate(ISettings settings, string name, DateTime birthDate)
        {
            return await Task.WhenAll(
                (await _dataFactory.GetByNameBirthDate(new CommonCore.DataSettings(settings), name, birthDate))
                .Select<LoanData, Task<Loan>>(d => Create(settings, d)));
        }

        public async Task<IEnumerable<ILoan>> GetWithUnprocessedPayments(ISettings settings)
        {
            return await Task.WhenAll(
                (await _dataFactory.GetWithUnprocessedPayments(new CommonCore.DataSettings(settings)))
                .Select<LoanData, Task<Loan>>(d => Create(settings, d)));
        }
    }
}
