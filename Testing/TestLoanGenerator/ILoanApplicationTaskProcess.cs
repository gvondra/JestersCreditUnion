using JestersCreditUnion.Interface.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationTaskProcess : IDisposable
    {
        void AddLoanApplication(LoanApplication application);
    }
}
