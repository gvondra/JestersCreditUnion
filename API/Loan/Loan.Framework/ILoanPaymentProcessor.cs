using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanPaymentProcessor
    {
        Task Process(ISettings settings, ILoan loan);
    }
}
