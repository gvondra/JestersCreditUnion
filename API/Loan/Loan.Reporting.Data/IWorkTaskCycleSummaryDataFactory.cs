using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data
{
    public interface IWorkTaskCycleSummaryDataFactory
    {
        Task<IEnumerable<WorkTaskCycleSummaryData>> GetSummary(ISettings settings, DateTime minCreateDate);
    }
}
