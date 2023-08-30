using JestersCreditUnion.Interface.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanProcess : IDisposable
    {   
        void AddLoanApplication(LoanApplication application);
        void Shutdown();
    }
}
