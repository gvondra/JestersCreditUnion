using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface IWorkTaskCycleSummaryFactory
    {
        Task<IEnumerable<WorkTaskCycleSummaryItem>> GetSummary(ISettings settings, DateTime minCreateDate);
    }
}
