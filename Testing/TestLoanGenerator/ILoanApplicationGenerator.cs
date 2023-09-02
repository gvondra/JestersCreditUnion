using JestersCreditUnion.Interface.Models;
using System.Collections.Generic;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationGenerator : IEnumerable<LoanApplication>
    {
        LoanApplication GenerateLoanApplication();
    }
}
