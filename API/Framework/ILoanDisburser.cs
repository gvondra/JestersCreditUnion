using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanDisburser
    {
        Task<ITransaction> Disburse(ISettings settings, ILoan loan);
    }
}
