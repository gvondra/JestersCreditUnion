using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplicationFactory
    {
        IAddressFactory AddressFactory { get; set; }
        IEmailAddressFactory EmailAddressFactory { get; set; }
        IPhoneFactory PhoneFactory { get; set; }

        ILoanApplication Create(Guid userId);
        Task<ILoanApplication> Get(ISettings settings, Guid id);
    }
}
