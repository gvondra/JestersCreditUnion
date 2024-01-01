using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanApplicationFactory
    {
        IAddressFactory AddressFactory { get; }

        ILoanApplication Create(Guid userId);
        Task<ILoanApplication> Get(ISettings settings, Guid id);
        Task<IEnumerable<ILoanApplication>> GetByUserId(ISettings settings, Guid userId);
    }
}
