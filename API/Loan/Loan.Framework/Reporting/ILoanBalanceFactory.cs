using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface ILoanBalanceFactory
    {
        Task<IEnumerable<LoanPastDue>> GetLoansPastDue(ISettings settings, short minDaysOverDue);
    }
}
