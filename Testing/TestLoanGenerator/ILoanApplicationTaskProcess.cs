using JestersCreditUnion.Interface.Loan.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationTaskProcess : ILoanApplicationProcessObserver, IDisposable
    {
        void AddObserver(ILoanApplicationProcessObserver observer);
        void AddLoanApplication(LoanApplication application);
        void WaitForProcessExit();
    }
}
