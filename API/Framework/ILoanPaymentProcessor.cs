using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanPaymentProcessor
    {
        Task Process(ISettings settings, ILoan loan);
    }
}
