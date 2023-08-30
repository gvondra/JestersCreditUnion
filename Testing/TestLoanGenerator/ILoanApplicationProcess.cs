using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ILoanApplicationProcess
    {
        void AddObserver(ILoanApplicationProcessObserver observer);
        Task GenerateLoanApplications();
    }
}
