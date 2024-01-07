using JestersCreditUnion.Interface.Loan.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface IDisburseLoanProcess : IDisposable
    {
        void AddLoan(Loan loan);
        void WaitForProcessExit();
    }
}
