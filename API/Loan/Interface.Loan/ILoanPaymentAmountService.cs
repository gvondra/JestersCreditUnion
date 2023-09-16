using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanPaymentAmountService
    {
        Task<LoanPaymentAmountResponse> Calculate(ISettings settings, LoanPaymentAmountRequest loanPyamentAmountRequest);
    }
}
