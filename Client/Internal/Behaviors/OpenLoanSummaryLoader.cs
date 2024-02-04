using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class OpenLoanSummaryLoader
    {
        private readonly OpenLoanSummaryVM _openLoanSummaryVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanSummaryService _loanSummaryService;

        public OpenLoanSummaryLoader(
            OpenLoanSummaryVM openLoanSummaryVM,
            ISettingsFactory settingsFactory,
            ILoanSummaryService loanSummaryService)
        {
            _openLoanSummaryVM = openLoanSummaryVM;
            _settingsFactory = settingsFactory;
            _loanSummaryService = loanSummaryService;
        }

        public void Load()
        {
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateLoanApi();
                return _loanSummaryService.GetOpenLoanSummary(settings).Result;
            })
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<OpenLoanSummary> load, object state)
        {
            try
            {
                OpenLoanSummary openLoanSummary = await load;
                _openLoanSummaryVM.LoanCount = openLoanSummary.LoanCount;
                _openLoanSummaryVM.MedianBalance = openLoanSummary.MedianBalance;
                _openLoanSummaryVM.Count60DaysOverdue = openLoanSummary.Count60DaysOverdue;
                _openLoanSummaryVM.Items = openLoanSummary.Items
                    .Select(i => new OpenLoanSummaryItemVM { Number = i.Number, Balance = i.Balance, NextPaymentDue = i.NextPaymentDue })
                    .ToList();
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
