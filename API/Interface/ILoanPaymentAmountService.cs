using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanPaymentAmountService
    {
        Task<LoanPaymentAmountResponse> Calculate(ISettings settings, LoanPaymentAmountRequest loanPyamentAmountRequest);
    }
}
