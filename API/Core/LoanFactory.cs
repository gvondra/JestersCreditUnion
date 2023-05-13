using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;

namespace JestersCreditUnion.Core
{
    public class LoanFactory : ILoanFactory
    {
        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }

        private Loan Create(LoanData data) => new Loan(data);

        public ILoan Create(ILoanApplication loanApplication)
        {
            return Create(
                new LoanData
                {
                    LoanId = Guid.NewGuid(),
                    Number = "1",
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
    }
}
