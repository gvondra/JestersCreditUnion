using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface IWorkTaskCycleService
    {
        Task<List<WorkTaskCycleSummaryItem>> GetSummary(ISettings settings, DateTime? minCreateDate = null);
    }
}
