using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanFactory
    {
        IAddressFactory AddressFactory { get; set; }
        IEmailAddressFactory EmailAddressFactory { get; set; }
        IPhoneFactory PhoneFactory { get; set; }

        ILoan Create(ILoanApplication loanApplication);
        Task<ILoan> Get(ISettings settings, Guid id);
        Task<ILoan> GetByNumber(ISettings settings, string number);
        Task<ILoan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
    }
}
