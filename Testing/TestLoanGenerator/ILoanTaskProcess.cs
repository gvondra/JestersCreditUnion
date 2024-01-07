using JestersCreditUnion.Interface.Loan.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanTaskProcess : IDisposable
    {
        void AddLoan(Loan loan);
    }
}
