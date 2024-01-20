using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressInterface = BrassLoon.Interface.Address;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplication : ILoanApplication
    {
        private readonly LoanApplicationData _data;
        private readonly ILoanApplicationDataSaver _dataSaver;
        private readonly ILoanApplicationFactory _factory;
        private readonly ILookupFactory _lookupFactory;
        private readonly IIdentificationCardDataSaver _identificationCardDataSaver;
        private readonly AddressInterface.IPhoneService _phoneService;
        private readonly AddressInterface.IEmailAddressService _emailService;
        private readonly SettingsFactory _settingsFactory;
        private readonly List<ILoanApplicationComment> _comments;
        private IAddress _borrowerAddress;
        private IAddress _coborrowerAddress;
        private ILoanApplicationDenial _loanApplicationDenial;
        private IdentificationCard _borrowerIdentificationCard;

        public LoanApplication(LoanApplicationData data,
            ILoanApplicationDataSaver dataSaver,
            ILoanApplicationFactory factory,
            ILookupFactory lookupFactory,
            IIdentificationCardDataSaver identificationCardDataSaver,
            AddressInterface.IPhoneService phoneService,
            AddressInterface.IEmailAddressService emailService,
            SettingsFactory settingsFactory)
        {
            _data = data;
            _dataSaver = dataSaver;
            _factory = factory;
            _lookupFactory = lookupFactory;
            _comments = InitiallizeComments(data.Comments, dataSaver);
            _identificationCardDataSaver = identificationCardDataSaver;
            _phoneService = phoneService;
            _emailService = emailService;
            _settingsFactory = settingsFactory;
        }

        public Guid LoanApplicationId => _data.LoanApplicationId;

        public Guid UserId => _data.UserId;

        public LoanApplicationStatus Status { get => (LoanApplicationStatus)_data.Status; set => _data.Status = (short)value; }
        public string BorrowerName { get => _data.BorrowerName; set => _data.BorrowerName = value; }
        public DateTime BorrowerBirthDate { get => _data.BorrowerBirthDate; set => _data.BorrowerBirthDate = value; }
        private Guid? BorrowerAddressId { get => _data.BorrowerAddressId; set => _data.BorrowerAddressId = value; }
        internal Guid? BorrowerEmailAddressId { get => _data.BorrowerEmailAddressId; set => _data.BorrowerEmailAddressId = value; }
        internal Guid? BorrowerPhoneId { get => _data.BorrowerPhoneId; set => _data.BorrowerPhoneId = value; }
        public string BorrowerEmployerName { get => _data.BorrowerEmployerName; set => _data.BorrowerEmployerName = value; }
        public DateTime? BorrowerEmploymentHireDate { get => _data.BorrowerEmploymentHireDate; set => _data.BorrowerEmploymentHireDate = value; }
        public decimal? BorrowerIncome { get => _data.BorrowerIncome; set => _data.BorrowerIncome = value; }
        private Guid? BorrowerIdentificationCardId { set => _data.BorrowerIdentificationCardId = value; }
        public string CoBorrowerName { get => _data.CoBorrowerName; set => _data.CoBorrowerName = value; }
        public DateTime? CoBorrowerBirthDate { get => _data.CoBorrowerBirthDate; set => _data.CoBorrowerBirthDate = value; }
        private Guid? CoBorrowerAddressId { get => _data.CoBorrowerAddressId; set => _data.CoBorrowerAddressId = value; }
        internal Guid? CoBorrowerEmailAddressId { get => _data.CoBorrowerEmailAddressId; set => _data.CoBorrowerEmailAddressId = value; }
        internal Guid? CoBorrowerPhoneId { get => _data.CoBorrowerPhoneId; set => _data.CoBorrowerPhoneId = value; }
        public string CoBorrowerEmployerName { get => _data.CoBorrowerEmployerName; set => _data.CoBorrowerEmployerName = value; }
        public DateTime? CoBorrowerEmploymentHireDate { get => _data.CoBorrowerEmploymentHireDate; set => _data.CoBorrowerEmploymentHireDate = value; }
        public decimal? CoBorrowerIncome { get => _data.CoBorrowerIncome; set => _data.CoBorrowerIncome = value; }
        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public string Purpose { get => _data.Purpose; set => _data.Purpose = value; }
        public decimal? MortgagePayment { get => _data.MortgagePayment; set => _data.MortgagePayment = value; }
        public decimal? RentPayment { get => _data.RentPayment; set => _data.RentPayment = value; }
        public string BorrowerEmailAddress { get; set; } = string.Empty;
        public string BorrowerPhone { get; set; } = string.Empty;
        public string CoBorrowerEmailAddress { get; set; } = string.Empty;
        public string CoBorrowerPhone { get; set; } = string.Empty;

        public DateTime? ClosedDate { get => _data.ClosedDate; set => _data.ClosedDate = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public DateTime ApplicationDate => _data.ApplicationDate;

        public List<ILoanApplicationComment> Comments => _comments;

        public ILoanApplicationDenial Denial
        {
            get
            {
                if (_loanApplicationDenial == null && _data.Denial != null)
                    _loanApplicationDenial = new LoanApplicationDenial(_data.Denial, _dataSaver, _lookupFactory);
                return _loanApplicationDenial;
            }
        }

        internal IdentificationCard BorrowerIdentificationCard
        {
            get
            {
                if (_borrowerIdentificationCard == null && _data.BorrowerIdentificationCard != null)
                    _borrowerIdentificationCard = new IdentificationCard(_data.BorrowerIdentificationCard, _identificationCardDataSaver);
                return _borrowerIdentificationCard;
            }
            private set => _borrowerIdentificationCard = value;
        }

        public async Task Create(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            await Task.WhenAll(new Task[]
            {
                SaveAddresses(transactionHandler, settings),
                SavePhones(settings),
                SaveEmailAddresses(settings)
            });
            await _dataSaver.Create(transactionHandler, _data);
        }

        public ILoanApplicationComment CreateComment(string text, Guid userId, bool isInternal = true)
        {
            LoanApplicationCommentData data = new LoanApplicationCommentData
            {
                IsInternal = isInternal,
                Text = text,
                LoanApplicationCommentId = Guid.NewGuid(),
                UserId = userId,
                CreateTimestamp = DateTime.UtcNow
            };
            return new LoanApplicationComment(data, _dataSaver, this);
        }

        public async Task<IAddress> GetBorrowerAddress(ISettings settings)
        {
            if (BorrowerAddressId.HasValue && (_borrowerAddress == null || !_borrowerAddress.AddressId.Equals(BorrowerAddressId.Value)))
            {
                _borrowerAddress = await _factory.AddressFactory.Get(settings, BorrowerAddressId.Value);
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
                _coborrowerAddress = await _factory.AddressFactory.Get(settings, CoBorrowerAddressId.Value);
            }
            return _coborrowerAddress;
        }

        public void SetCoBorrowerAddress(IAddress address)
        {
            _coborrowerAddress = address;
            CoBorrowerAddressId = null;
        }

        public async Task<string> GetStatusDescription(ISettings settings)
        {
            string result = null;
            ILookup lookup = await _lookupFactory.GetLookup(settings, typeof(LoanApplicationStatus));
            if (lookup != null)
            {
                result = lookup.GetDataValue(Status);
            }
            return result;
        }

        public async Task Update(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            if (_borrowerIdentificationCard != null)
            {
                if (_borrowerIdentificationCard.IsNew)
                    await _borrowerIdentificationCard.Create(transactionHandler);
                else
                    await _borrowerIdentificationCard.Update(transactionHandler);
                BorrowerIdentificationCardId = _borrowerIdentificationCard.IdentificationCardId;
            }
            await Task.WhenAll(new Task[]
            {
                SaveAddresses(transactionHandler, settings),
                SavePhones(settings),
                SaveEmailAddresses(settings)
            });
            await _dataSaver.Update(transactionHandler, _data);
        }

        private static List<ILoanApplicationComment> InitiallizeComments(IEnumerable<LoanApplicationCommentData> commentData, ILoanApplicationDataSaver dataSaver)
        {
            if (commentData != null)
            {
                return commentData.Select<LoanApplicationCommentData, ILoanApplicationComment>(d => new LoanApplicationComment(d, dataSaver))
                    .ToList();
            }
            else
            {
                return new List<ILoanApplicationComment>();
            }
        }

        public void SetDenial(LoanApplicationDenialReason reason, DateTime date, Guid userId, string text)
        {
            _loanApplicationDenial = null;
            _data.Denial = new LoanApplicationDenialData
            {
                Date = date,
                UserId = userId,
                Text = text,
                Reason = (short)reason
            };
            Status = LoanApplicationStatus.Denied;
            ClosedDate = date;
        }

        internal IdentificationCard SetBorrowerIdentificationCard(byte[] initializationVector, byte[] key)
        {
            IdentificationCard card = BorrowerIdentificationCard;
            if (card == null)
            {
                card = new IdentificationCard(
                    new IdentificationCardData
                    {
                        IdentificationCardId = _data.BorrowerIdentificationCardId ?? Guid.NewGuid()
                    },
                    _identificationCardDataSaver);
                BorrowerIdentificationCard = card;
            }
            card.InitializationVector = initializationVector;
            card.Key = key;
            return card;
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

        private async Task SaveAddresses(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            if (_borrowerAddress != null)
                await _borrowerAddress.Create(transactionHandler, settings);
            BorrowerAddressId = _borrowerAddress != null ? _borrowerAddress.AddressId : null;
            if (_coborrowerAddress != null)
                await _coborrowerAddress.Create(transactionHandler, settings);
            CoBorrowerAddressId = _coborrowerAddress != null ? _coborrowerAddress.AddressId : null;
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

        public IIdentificationCardSaver CreateIdentificationCardSaver() => new IdentificationCardSaver(this);
        public IIdentificationCardReader CreateIdentificationCardReader() => new IdentificationCardReader(this);

        public int GetBorrowerAge() => GetAge(BorrowerBirthDate, ApplicationDate);
        public int? GetCoBorrowerAge() => CoBorrowerBirthDate.HasValue ? GetAge(CoBorrowerBirthDate.Value, ApplicationDate) : null;

        private static int GetAge(DateTime birthDate, DateTime asOfDAte)
        {
            int age = asOfDAte.Year - birthDate.Year;
            if (asOfDAte.Month < birthDate.Month
                || (asOfDAte.Month == birthDate.Month && asOfDAte.Day < birthDate.Day))
            {
                age -= 1;
            }
            return age;
        }

        public Task<IRating> GetRating(ISettings settings) => _factory.RatingFactory.GetLoanApplication(settings, LoanApplicationId);
    }
}
