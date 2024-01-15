using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanApplicationRaterFactory
    {
        Task<ILoanApplicationRater> Create();
    }
}
