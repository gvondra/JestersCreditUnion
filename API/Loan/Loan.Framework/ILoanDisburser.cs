using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanDisburser
    {
        Task<ITransaction> Disburse(ISettings settings, ILoan loan);
    }
}
