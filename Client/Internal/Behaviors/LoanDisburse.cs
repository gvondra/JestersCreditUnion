using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class LoanDisburse : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is LoanVM loanVM)
            {
                _canExecute = false;
                CanExecuteChanged.Invoke(this, new EventArgs());
                Task.Run(() => Disburse(loanVM.LoanId.Value))
                    .ContinueWith(DisburseCallback, loanVM, TaskScheduler.FromCurrentSynchronizationContext());
                loanVM.DisburseVisibility = Visibility.Collapsed;
            }
        }

        private DisburseResponse Disburse(Guid loanId)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateLoanApi();
                ILoanService loanService = scope.Resolve<ILoanService>();
                return loanService.DisburseFunds(settings, loanId).Result;
            }
        }

        private async Task DisburseCallback(Task<DisburseResponse> disburse, object state)
        {
            LoanVM loanVM = (LoanVM)state;
            try
            {                
                DisburseResponse disburseResponse = await disburse;
                loanVM.FirstPaymentDue = disburseResponse.Loan.FirstPaymentDue;
                loanVM.NextPaymentDue = disburseResponse.Loan.NextPaymentDue;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
                loanVM.DisburseVisibility = Visibility.Visible;
            }
            finally
            {
                _canExecute = true;
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }
    }
}
