using JestersCreditUnion.Interface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanPaymentService
    {
        Task<List<LoanPayment>> Save(ISettings settings, IEnumerable<LoanPayment> loanPayments);
    }
}
