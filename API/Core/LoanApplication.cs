using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Core
{
    public class LoanApplication : ILoanApplication
    {
        private readonly LoanApplicationData _data;
        private readonly ILoanApplicationDataSaver _dataSaver;
        private readonly ILoanApplicationFactory _factory;
        private readonly ILookupFactory _lookupFactory;
        private IAddress _borrowerAddress = null;
        private IAddress _coborrowerAddress = null;
        private IEmailAddress _borrowerEmailAddress = null;
        private IEmailAddress _coborrowerEmailAddress = null;
        private IPhone _borrowerPhone = null;
        private IPhone _coborrowerPhone = null;
        private List<ILoanApplicationComment> _comments = null;
        private ILoanApplicationDenial _loanApplicationDenial = null;

        public LoanApplication(LoanApplicationData data, 
            ILoanApplicationDataSaver dataSaver, 
            ILoanApplicationFactory factory, 
            ILookupFactory lookupFactory)
        {
            _data = data;
            _dataSaver = dataSaver;
            _factory = factory;
            _lookupFactory = lookupFactory;
            _comments = InitiallizeComments(data.Comments, dataSaver);
        }

        public Guid LoanApplicationId => _data.LoanApplicationId;

        public Guid UserId => _data.UserId;

        public LoanApplicationStatus Status { get => (LoanApplicationStatus)_data.Status; set => _data.Status = (short)value; }
        public string BorrowerName { get => _data.BorrowerName; set => _data.BorrowerName = value; }
        public DateTime BorrowerBirthDate { get => _data.BorrowerBirthDate; set => _data.BorrowerBirthDate = value; }
        public Guid? BorrowerAddressId { get => _data.BorrowerAddressId; set => _data.BorrowerAddressId = value; }
        public Guid? BorrowerEmailAddressId { get => _data.BorrowerEmailAddressId; set => _data.BorrowerEmailAddressId = value; }
        public Guid? BorrowerPhoneId { get => _data.BorrowerPhoneId; set => _data.BorrowerPhoneId = value; }
        public string BorrowerEmployerName { get => _data.BorrowerEmployerName; set => _data.BorrowerEmployerName = value; }
        public DateTime? BorrowerEmploymentHireDate { get => _data.BorrowerEmploymentHireDate; set => _data.BorrowerEmploymentHireDate = value; }
        public decimal? BorrowerIncome { get => _data.BorrowerIncome; set => _data.BorrowerIncome = value; }
        public string CoBorrowerName { get => _data.CoBorrowerName; set => _data.CoBorrowerName = value; }
        public DateTime? CoBorrowerBirthDate { get => _data.CoBorrowerBirthDate; set => _data.CoBorrowerBirthDate = value; }
        public Guid? CoBorrowerAddressId { get => _data.CoBorrowerAddressId; set => _data.CoBorrowerAddressId = value; }
        public Guid? CoBorrowerEmailAddressId { get => _data.CoBorrowerEmailAddressId; set => _data.CoBorrowerEmailAddressId = value; }
        public Guid? CoBorrowerPhoneId { get => _data.CoBorrowerPhoneId; set => _data.CoBorrowerPhoneId = value; }
        public string CoBorrowerEmployerName { get => _data.CoBorrowerEmployerName; set => _data.CoBorrowerEmployerName = value; }
        public DateTime? CoBorrowerEmploymentHireDate { get => _data.CoBorrowerEmploymentHireDate; set => _data.CoBorrowerEmploymentHireDate = value; }
        public decimal? CoBorrowerIncome { get => _data.CoBorrowerIncome; set => _data.CoBorrowerIncome = value; }
        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public string Purpose { get => _data.Purpose; set => _data.Purpose = value; }
        public decimal? MortgagePayment { get => _data.MortgagePayment; set => _data.MortgagePayment = value; }
        public decimal? RentPayment { get => _data.RentPayment; set => _data.RentPayment = value; }

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

        public Task Create(CommonCore.ITransactionHandler transactionHandler) => _dataSaver.Create(transactionHandler, _data);

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

        public async Task<IEmailAddress> GetBorrowerEmailAddress(ISettings settings)
        {
            if (BorrowerEmailAddressId.HasValue && (_borrowerEmailAddress == null || !_borrowerEmailAddress.EmailAddressId.Equals(BorrowerEmailAddressId.Value)))
            {
                _borrowerEmailAddress = await _factory.EmailAddressFactory.Get(settings, BorrowerEmailAddressId.Value);
            }
            return _borrowerEmailAddress;
        }

        public async Task<IPhone> GetBorrowerPhone(ISettings settings)
        {
            if (BorrowerPhoneId.HasValue && (_borrowerPhone == null || !_borrowerPhone.PhoneId.Equals(BorrowerPhoneId.Value)))
            {
                _borrowerPhone = await _factory.PhoneFactory.Get(settings, BorrowerPhoneId.Value);
            }
            return _borrowerPhone;
        }

        public async Task<IAddress> GetCoBorrowerAddress(ISettings settings)
        {
            if (CoBorrowerAddressId.HasValue && (_coborrowerAddress == null || !_coborrowerAddress.AddressId.Equals(CoBorrowerAddressId.Value)))
            {
                _coborrowerAddress = await _factory.AddressFactory.Get(settings, CoBorrowerAddressId.Value);
            }
            return _coborrowerAddress;
        }

        public async Task<IEmailAddress> GetCoBorrowerEmailAddress(ISettings settings)
        {
            if (CoBorrowerEmailAddressId.HasValue && (_coborrowerEmailAddress == null || !_coborrowerEmailAddress.EmailAddressId.Equals(CoBorrowerEmailAddressId.Value)))
            {
                _coborrowerEmailAddress = await _factory.EmailAddressFactory.Get(settings, CoBorrowerEmailAddressId.Value);
            }
            return _coborrowerEmailAddress;
        }

        public async Task<IPhone> GetCoBorrowerPhone(ISettings settings)
        {
            if (CoBorrowerPhoneId.HasValue && (_coborrowerPhone == null || !_coborrowerPhone.PhoneId.Equals(CoBorrowerPhoneId.Value)))
            {
                _coborrowerPhone = await _factory.PhoneFactory.Get(settings, CoBorrowerPhoneId.Value);
            }
            return _coborrowerPhone;
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

        public Task Update(CommonCore.ITransactionHandler transactionHandler) => _dataSaver.Update(transactionHandler, _data);

        private static List<ILoanApplicationComment> InitiallizeComments(IEnumerable<LoanApplicationCommentData> commentData, ILoanApplicationDataSaver dataSaver)
        {
            if (commentData != null)
                return commentData.Select<LoanApplicationCommentData, ILoanApplicationComment>(d => new LoanApplicationComment(d, dataSaver))
                    .ToList();
            else
                return new List<ILoanApplicationComment>();
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
            if (!string.IsNullOrEmpty(text)) 
                Status = LoanApplicationStatus.Denied;
        }
    }
}
