using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanProcessObserver
    {
        Task LoanCreated(ILoanProcess loanProcess, params Loan[] loans);
    }
}
