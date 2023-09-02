using JestersCreditUnion.Interface.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface IDisburseLoanProcess : IDisposable
    {
        void AddLoan(Loan loan);
        void WaitForProcessExit();
    }
}
