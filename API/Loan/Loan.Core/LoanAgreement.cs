using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Threading.Tasks;
using AddressInterface = BrassLoon.Interface.Address;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanAgreement : ILoanAgreement
    {
        private readonly LoanAgreementData _data;
        private readonly ILoanAgreementDataSaver _dataSaver;
        private readonly ILoanFactory _loanFactory;
        private readonly ILoan _loan;
        private readonly AddressInterface.IPhoneService _phoneService;
        private readonly AddressInterface.IEmailAddressService _emailService;
        private readonly SettingsFactory _settingsFactory;
        private IAddress _borrowerAddress;
        private IAddress _coborrowerAddress;

        public LoanAgreement(
            LoanAgreementData data,
            SettingsFactory settingsFactory,
            AddressInterface.IPhoneService phoneService,
            AddressInterface.IEmailAddressService emailService,
            ILoanAgreementDataSaver dataSaver,
            ILoanFactory loanFactory,
            ILoan loan)
        {
            _data = data;
            _settingsFactory = settingsFactory;
            _phoneService = phoneService;
            _emailService = emailService;
            _dataSaver = dataSaver;
            _loanFactory = loanFactory;
            _loan = loan;
        }

        public LoanAgreement(
            LoanAgreementData data,
            SettingsFactory settingsFactory,
            AddressInterface.IPhoneService phoneService,
            AddressInterface.IEmailAddressService emailService,
            ILoanAgreementDataSaver dataSaver,
            ILoanFactory loanFactory)
            : this(data, settingsFactory, phoneService, emailService, dataSaver, loanFactory, null)
        { }

        public Guid LoanId { get; private set; }

        public LoanAgrementStatus Status { get => (LoanAgrementStatus)_data.Status; set => _data.Status = (short)value; }

        public DateTime CreateDate => _data.CreateDate;

        public DateTime? AgreementDate { get => _data.AgreementDate; set => _data.AgreementDate = value; }
        public string BorrowerName { get => _data.BorrowerName; set => _data.BorrowerName = value; }
        public DateTime BorrowerBirthDate { get => _data.BorrowerBirthDate; set => _data.BorrowerBirthDate = value; }
        public Guid? BorrowerAddressId { get => _data.BorrowerAddressId; set => _data.BorrowerAddressId = value; }
        internal Guid? BorrowerEmailAddressId { get => _data.BorrowerEmailAddressId; set => _data.BorrowerEmailAddressId = value; }
        public string BorrowerEmailAddress { get; set; } = string.Empty;
        internal Guid? BorrowerPhoneId { get => _data.BorrowerPhoneId; set => _data.BorrowerPhoneId = value; }
        public string BorrowerPhone { get; set; } = string.Empty;
        public string CoBorrowerName { get => _data.CoBorrowerName; set => _data.CoBorrowerName = value; }
        public DateTime? CoBorrowerBirthDate { get => _data.CoBorrowerBirthDate; set => _data.CoBorrowerBirthDate = value; }
        public Guid? CoBorrowerAddressId { get => _data.CoBorrowerAddressId; set => _data.CoBorrowerAddressId = value; }
        internal Guid? CoBorrowerEmailAddressId { get => _data.CoBorrowerEmailAddressId; set => _data.CoBorrowerEmailAddressId = value; }
        public string CoBorrowerEmailAddress { get; set; } = string.Empty;
        internal Guid? CoBorrowerPhoneId { get => _data.CoBorrowerPhoneId; set => _data.CoBorrowerPhoneId = value; }
        public string CoBorrowerPhone { get; set; } = string.Empty;
        public decimal OriginalAmount { get => _data.OriginalAmount; set => _data.OriginalAmount = value; }
        public short OriginalTerm { get => _data.OriginalTerm; set => _data.OriginalTerm = value; }
        public decimal InterestRate { get => _data.InterestRate; set => _data.InterestRate = value; }
        public decimal PaymentAmount { get => _data.PaymentAmount; set => _data.PaymentAmount = value; }
        public LoanPaymentFrequency PaymentFrequency { get => (LoanPaymentFrequency)_data.PaymentFrequency; set => _data.PaymentFrequency = (short)value; }

        public async Task Create(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            if (_loan == null)
                throw new ApplicationException("Cannot create loan agreement. No parent loan was set");
            LoanId = _loan.LoanId;
            await Task.WhenAll(
                SaveAddresses(transactionHandler, settings),
                SaveEmailAddresses(settings),
                SavePhones(settings));
            await _dataSaver.Create(transactionHandler, _data);
        }

        public async Task<IAddress> GetBorrowerAddress(ISettings settings)
        {
            if (BorrowerAddressId.HasValue && (_borrowerAddress == null || !_borrowerAddress.AddressId.Equals(BorrowerAddressId.Value)))
            {
                _borrowerAddress = await _loanFactory.AddressFactory.Get(settings, BorrowerAddressId.Value);
            }
            return _borrowerAddress;
        }

        public void SetBorrowerAddress(IAddress address)
        {
            _borrowerAddress = address;
            BorrowerAddressId = null;
        }

        public async Task<IAddress> GetCoBorrowerAddress(ISettings settings)
        {
            if (CoBorrowerAddressId.HasValue && (_coborrowerAddress == null || !_coborrowerAddress.AddressId.Equals(CoBorrowerAddressId.Value)))
            {
                _coborrowerAddress = await _loanFactory.AddressFactory.Get(settings, CoBorrowerAddressId.Value);
            }
            return _coborrowerAddress;
        }

        public void SetCoBorrowerAddress(IAddress address)
        {
            _coborrowerAddress = address;
            CoBorrowerAddressId = null;
        }

        public async Task Update(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            await Task.WhenAll(
                SaveAddresses(transactionHandler, settings),
                SaveEmailAddresses(settings),
                SavePhones(settings));
            await _dataSaver.Update(transactionHandler, _data);
        }

        private async Task SaveAddresses(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            if (_borrowerAddress != null)
                await _borrowerAddress.Create(transactionHandler, settings);
            BorrowerAddressId = _borrowerAddress != null ? _borrowerAddress.AddressId : null;
            if (_coborrowerAddress != null)
                await _coborrowerAddress.Create(transactionHandler, settings);
            CoBorrowerAddressId = _coborrowerAddress != null ? _coborrowerAddress.AddressId : null;
        }

        private async Task SaveEmailAddresses(ISettings settings)
        {
            AddressInterface.Models.EmailAddress email;
            if (!string.IsNullOrEmpty(BorrowerEmailAddress))
            {
                email = new AddressInterface.Models.EmailAddress
                {
                    DomainId = settings.AddressDomainId.Value,
                    Address = BorrowerEmailAddress
                };
                email = await _emailService.Save(_settingsFactory.CreateAddress(settings), email);
                BorrowerEmailAddressId = email.EmailAddressId;
            }
            else
            {
                BorrowerEmailAddressId = null;
            }
            if (!string.IsNullOrEmpty(CoBorrowerEmailAddress))
            {
                email = new AddressInterface.Models.EmailAddress
                {
                    DomainId = settings.AddressDomainId.Value,
                    Address = CoBorrowerEmailAddress
                };
                email = await _emailService.Save(_settingsFactory.CreateAddress(settings), email);
                CoBorrowerEmailAddressId = email.EmailAddressId;
            }
            else
            {
                CoBorrowerEmailAddressId = null;
            }
        }

        private async Task SavePhones(ISettings settings)
        {
            AddressInterface.Models.Phone phone;
            if (!string.IsNullOrEmpty(BorrowerPhone))
            {
                phone = new AddressInterface.Models.Phone
                {
                    DomainId = settings.AddressDomainId.Value,
                    Number = BorrowerPhone
                };
                phone = await _phoneService.Save(_settingsFactory.CreateAddress(settings), phone);
                BorrowerPhoneId = phone.PhoneId;
            }
            else
            {
                BorrowerPhoneId = null;
            }
            if (!string.IsNullOrEmpty(CoBorrowerPhone))
            {
                phone = new AddressInterface.Models.Phone
                {
                    DomainId = settings.AddressDomainId.Value,
                    Number = CoBorrowerPhone
                };
                phone = await _phoneService.Save(_settingsFactory.CreateAddress(settings), phone);
                CoBorrowerPhoneId = phone.PhoneId;
            }
            else
            {
                CoBorrowerPhoneId = null;
            }
        }
    }
}
