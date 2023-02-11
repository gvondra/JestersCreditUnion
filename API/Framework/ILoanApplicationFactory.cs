using System;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplicationFactory
    {
        ILoanApplication Create(Guid userId);
    }
}
