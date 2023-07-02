using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanSaver
    {
        Task Create(ISettings settings, ILoan loan);
        Task Update(ISettings settings, ILoan loan);
        Task DisburseFundsUpdate(ISettings settings, ILoan loan);
    }
}
