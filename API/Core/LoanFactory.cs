using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanFactory : ILoanFactory
    {
        private readonly ILoanDataFactory _dataFactory;
        private readonly ILoanDataSaver _dataSaver;
        private readonly LoanNumberGenerator _numberGenerator;

        public LoanFactory(ILoanDataFactory dataFactory,
            ILoanDataSaver dataSaver,
            LoanNumberGenerator numberGenerator)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _numberGenerator = numberGenerator;
        }

        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }

        private Loan Create(LoanData data) => new Loan(data, _dataSaver);

        public ILoan Create(ILoanApplication loanApplication)
        {
            return Create(
                new LoanData
                {
                    LoanId = Guid.NewGuid(),
                    Number = _numberGenerator.Generate(),
                    Agreement = new LoanAgreementData
                    {
                        AgreementDate = DateTime.Today,
                        BorrowerAddressId = loanApplication.BorrowerAddressId,
                        BorrowerBirthDate = loanApplication.BorrowerBirthDate,
                        BorrowerEmailAddressId = loanApplication.BorrowerEmailAddressId,
                        BorrowerName = loanApplication.BorrowerName,
                        BorrowerPhoneId = loanApplication.BorrowerPhoneId,
                        CoBorrowerAddressId = loanApplication.CoBorrowerAddressId,
                        CoBorrowerBirthDate = loanApplication.CoBorrowerBirthDate,
                        CoBorrowerEmailAddressId = loanApplication.CoBorrowerEmailAddressId,
                        CoBorrowerName = loanApplication.CoBorrowerName,
                        CoBorrowerPhoneId = loanApplication.CoBorrowerPhoneId,
                        OriginalAmount = loanApplication.Amount
                    }
                });
        }

        public async Task<ILoan> GetByNumber(ISettings settings, string number)
        {
            Loan result = null;
            LoanData data = await _dataFactory.GetByNumber(new DataSettings(settings), number);
            if (data != null)
                result = Create(data);
            return result;
        }
    }
}
