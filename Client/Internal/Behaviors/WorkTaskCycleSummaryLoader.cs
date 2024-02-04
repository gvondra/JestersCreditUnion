using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskCycleSummaryLoader
    {
        private readonly WorkTaskCycleSummaryVM _workTaskCycleSummaryVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskCycleService _workTaskCycleService;

        public WorkTaskCycleSummaryLoader(WorkTaskCycleSummaryVM workTaskCycleSummaryVM, ISettingsFactory settingsFactory, IWorkTaskCycleService workTaskCycleService)
        {
            _workTaskCycleSummaryVM = workTaskCycleSummaryVM;
            _settingsFactory = settingsFactory;
            _workTaskCycleService = workTaskCycleService;
        }

        public void Load()
        {
            _workTaskCycleSummaryVM.Items.Clear();
            Task.Run(() => _workTaskCycleService.GetSummary(_settingsFactory.CreateLoanApi()).Result)
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<WorkTaskCycleSummaryItem>> load, object state)
        {
            try
            {
                List<WorkTaskCycleSummaryItem> items = await load;
                _workTaskCycleSummaryVM.Items.Clear();
                foreach (WorkTaskCycleSummaryItem item in items)
                {
                    _workTaskCycleSummaryVM.Items.Add(new WorkTaskCycleSummaryItemVM(item));
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
