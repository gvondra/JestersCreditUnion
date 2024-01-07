using JestersCreditUnion.Interface.Loan.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanProcess : IDisposable
    {
        void AddObserver(ILoanProcessObserver observer);
        void AddLoanApplication(LoanApplication application);
        void WaitForProcessExit();
    }
}
