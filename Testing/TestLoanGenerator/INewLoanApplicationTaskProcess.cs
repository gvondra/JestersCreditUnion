using JestersCreditUnion.Interface.Loan.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface INewLoanApplicationTaskProcess : ILoanApplicationProcessObserver, IDisposable
    {
        int Count { get; }
        void AddObserver(ILoanApplicationProcessObserver observer);
        void AddLoanApplication(LoanApplication application);
        void WaitForProcessExit();
    }
}
