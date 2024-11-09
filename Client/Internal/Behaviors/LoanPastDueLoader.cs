using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class LoanPastDueLoader
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanReportService _loanReportService;
        private readonly LoanPastDueVM _loanPastDueVM;

        public LoanPastDueLoader(ISettingsFactory settingsFactory, ILoanReportService loanReportService, LoanPastDueVM loanPastDueVM)
        {
            _settingsFactory = settingsFactory;
            _loanReportService = loanReportService;
            _loanPastDueVM = loanPastDueVM;
        }

        public void Load()
        {
            _loanPastDueVM.Items.Clear();
            Task.Run(() => _loanReportService.GetPastDue(_settingsFactory.CreateLoanApi()).Result)
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<LoanPastDue>> load, object state)
        {
            try
            {
                List<LoanPastDue> loanPastDueItems = await load;
                _loanPastDueVM.Items.Clear();
                foreach (LoanPastDue loanPastDue in await load)
                {
                    _loanPastDueVM.Items.Add(loanPastDue);
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
