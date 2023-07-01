using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanAgreement : ILoanAgreement
    {
        private readonly LoanAgreementData _data;
        private readonly ILoanAgreementDataSaver _dataSaver;
        private readonly ILoanFactory _loanFactory;
        private readonly ILoan _loan;

        public LoanAgreement(
            LoanAgreementData data,
            ILoanAgreementDataSaver dataSaver,
            ILoanFactory loanFactory,
            ILoan loan)
        {
            _data = data;
            _dataSaver = dataSaver;
            _loanFactory = loanFactory;
            _loan = loan;
        }

        public LoanAgreement(
            LoanAgreementData data,
            ILoanAgreementDataSaver dataSaver,
            ILoanFactory loanFactory)
            : this(data, dataSaver, loanFactory, null)
        { }

        public Guid LoanId { get; private set; }

        public LoanAgrementStatus Status { get => (LoanAgrementStatus)_data.Status; set => _data.Status = (short)value; }

        public DateTime CreateDate => _data.CreateDate;

        public DateTime? AgreementDate { get => _data.AgreementDate; set => _data.AgreementDate = value; }
        public string BorrowerName { get => _data.BorrowerName; set => _data.BorrowerName = value; }
        public DateTime BorrowerBirthDate { get => _data.BorrowerBirthDate; set => _data.BorrowerBirthDate = value; }
        public Guid? BorrowerAddressId { get => _data.BorrowerAddressId; set => _data.BorrowerAddressId = value; }
        public Guid? BorrowerEmailAddressId { get => _data.BorrowerEmailAddressId; set => _data.BorrowerEmailAddressId = value; }
        public Guid? BorrowerPhoneId { get => _data.BorrowerPhoneId; set => _data.BorrowerPhoneId = value; }
        public string CoBorrowerName { get => _data.CoBorrowerName; set => _data.CoBorrowerName = value; }
        public DateTime? CoBorrowerBirthDate { get => _data.CoBorrowerBirthDate; set => _data.CoBorrowerBirthDate = value; }
        public Guid? CoBorrowerAddressId { get => _data.CoBorrowerAddressId; set => _data.CoBorrowerAddressId = value; }
        public Guid? CoBorrowerEmailAddressId { get => _data.CoBorrowerEmailAddressId; set => _data.CoBorrowerEmailAddressId = value; }
        public Guid? CoBorrowerPhoneId { get => _data.CoBorrowerPhoneId; set => _data.CoBorrowerPhoneId = value; }
        public decimal OriginalAmount { get => _data.OriginalAmount; set => _data.OriginalAmount = value; }
        public short OriginalTerm { get => _data.OriginalTerm; set => _data.OriginalTerm = value; }
        public decimal InterestRate { get => _data.InterestRate; set => _data.InterestRate = value; }
        public decimal PaymentAmount { get => _data.PaymentAmount; set => _data.PaymentAmount = value; }
        public short PaymentFrequency { get => _data.PaymentFrequency; set => _data.PaymentFrequency = value; }

        public async Task Create(CommonCore.ITransactionHandler transactionHandler)
        {
            if (_loan == null)
                throw new ApplicationException("Cannot create loan agreement. No parent loan was set");
            LoanId = _loan.LoanId;
            await _dataSaver.Create(transactionHandler, _data);
        }

        public Task<IAddress> GetBorrowerAddress(ISettings settings)
        {
            if (BorrowerAddressId.HasValue)
                return _loanFactory.AddressFactory.Get(settings, BorrowerAddressId.Value);
            else
                return Task.FromResult<IAddress>(null);
        }

        public Task<IEmailAddress> GetBorrowerEmailAddress(ISettings settings)
        {
            if (BorrowerEmailAddressId.HasValue)
                return _loanFactory.EmailAddressFactory.Get(settings, BorrowerEmailAddressId.Value);
            else
                return Task.FromResult<IEmailAddress>(null);
        }

        public Task<IPhone> GetBorrowerPhone(ISettings settings)
        {
            if (BorrowerPhoneId.HasValue)
                return _loanFactory.PhoneFactory.Get(settings, BorrowerPhoneId.Value);
            else
                return Task.FromResult<IPhone>(null);
        }

        public Task<IAddress> GetCoBorrowerAddress(ISettings settings)
        {
            if (CoBorrowerAddressId.HasValue)
                return _loanFactory.AddressFactory.Get(settings, CoBorrowerAddressId.Value);
            else
                return Task.FromResult<IAddress>(null);
        }

        public Task<IEmailAddress> GetCoBorrowerEmailAddress(ISettings settings)
        {
            if (CoBorrowerEmailAddressId.HasValue)
                return _loanFactory.EmailAddressFactory.Get(settings, CoBorrowerEmailAddressId.Value);
            else
                return Task.FromResult<IEmailAddress>(null);
        }

        public Task<IPhone> GetCoBorrowerPhone(ISettings settings)
        {
            if (CoBorrowerPhoneId.HasValue)
                return _loanFactory.PhoneFactory.Get(settings, CoBorrowerPhoneId.Value);
            else
                return Task.FromResult<IPhone>(null);
        }

        public Task Update(CommonCore.ITransactionHandler transactionHandler)
        {
            return _dataSaver.Update(transactionHandler, _data);
        }
    }
}
