using System;
using System.Collections.Generic;
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
        Task<IEnumerable<ILoanApplication>> GetByUserId(ISettings settings, Guid userId);
    }
}
