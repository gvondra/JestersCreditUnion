using JestersCreditUnion.Interface.Loan.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationTaskProcess : IDisposable
    {
        void AddLoanApplication(LoanApplication application);
    }
}
