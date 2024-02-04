using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Reporting;
using JestersCreditUnion.Loan.Reporting.Data;
using JestersCreditUnion.Loan.Reporting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class WorkTaskCycleSummaryFactory : IWorkTaskCycleSummaryFactory
    {
        private readonly IWorkTaskCycleSummaryDataFactory _dataFactory;

        public WorkTaskCycleSummaryFactory(IWorkTaskCycleSummaryDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        private static WorkTaskCycleSummaryItem Create(WorkTaskCycleSummaryData data)
        {
            return new WorkTaskCycleSummaryItem
            {
                CreateYear = data.CreateYear,
                CreateMonth = data.CreateMonth,
                AssignedYear = data.AssignedYear,
                AssignedMonth = data.AssignedMonth,
                DaysToAssigment = data.DaysToAssigment,
                ClosedYear = data.ClosedYear,
                ClosedMonth = data.ClosedMonth,
                DaysToClose = data.DaysToClose,
                TotalDays = data.TotalDays,
                Title = data.Title,
                Count = data.Count
            };
        }

        public async Task<IEnumerable<WorkTaskCycleSummaryItem>> GetSummary(Framework.ISettings settings, DateTime minCreateDate)
        {
            return (await _dataFactory.GetSummary(new DataSettings(settings), minCreateDate))
                .Select<WorkTaskCycleSummaryData, WorkTaskCycleSummaryItem>(Create);
        }
    }
}
