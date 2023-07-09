using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanFactory
    {
        IAddressFactory AddressFactory { get; set; }
        IEmailAddressFactory EmailAddressFactory { get; set; }
        IPhoneFactory PhoneFactory { get; set; }
        ITransactionFacatory TransactionFacatory { get; set; }

        ILoan Create(ILoanApplication loanApplication);
        Task<ILoan> Get(ISettings settings, Guid id);
        Task<ILoan> GetByNumber(ISettings settings, string number);
        Task<IEnumerable<ILoan>> GetByNameBirthDate(ISettings settings, string name, DateTime birthDate);
        Task<ILoan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
    }
}
