using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanReportService
    {
        Task<List<LoanPastDue>> GetPastDue(ISettings settings, short minDays = 60);
    }
}
