using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.Behaviors
{
    public class LoanAmortizationLoader
    {
        private readonly LoanAmortizationVM _loanAmortizationVM;

        public LoanAmortizationLoader(LoanAmortizationVM loanAmortizationVM)
        {
            _loanAmortizationVM = loanAmortizationVM;
        }

        public void Load()
        {
            _loanAmortizationVM.BusyVisibility = Visibility.Visible;
            _loanAmortizationVM.AmortizationItems.Clear();
            Task.Run(() =>
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    IAmortizationService amortizationService = scope.Resolve<IAmortizationService>();
                    return amortizationService.Get(settingsFactory.CreateLoanApi(), _loanAmortizationVM.LoanId).Result;
                }
            }).ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<AmortizationItem>> load, object state)
        {
            try
            {
                List<AmortizationItem> items = await load;
                _loanAmortizationVM.AmortizationItems.Clear();
                foreach (AmortizationItem item in items)
                {
                    _loanAmortizationVM.AmortizationItems.Add(
                        AmortizationItemVM.Create(item));
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _loanAmortizationVM.BusyVisibility = Visibility.Collapsed;
            }
        }
    }
}
