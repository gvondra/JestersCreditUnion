using JestersCreditUnion.Loan.Framework;
using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Core.Rate.LoanApplication
{
    internal interface IComponent
    {
        int Points { get; }
        IEnumerable<IRatingLog> Evaluate(ILoanApplication loanApplication, int totalPoints);
    }
}
