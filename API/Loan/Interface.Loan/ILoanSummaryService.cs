using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanSummaryService
    {
        Task<OpenLoanSummary> GetOpenLoanSummary(ISettings settings);
    }
}
