using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplicationSaver
    {
        Task Create(ISettings settings, ILoanApplication loanApplication);
    }
}
