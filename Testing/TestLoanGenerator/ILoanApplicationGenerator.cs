using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.Generic;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationGenerator : IEnumerable<LoanApplication>
    {
        LoanApplication GenerateLoanApplication();
    }
}
