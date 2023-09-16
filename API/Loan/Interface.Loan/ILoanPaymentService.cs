using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanPaymentService
    {
        Task<List<LoanPayment>> Save(ISettings settings, IEnumerable<LoanPayment> loanPayments);
    }
}
