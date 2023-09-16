using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationUnderReview : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(LoanApplicationVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Parameter must be of type {typeof(LoanApplicationVM).Name}");
            LoanApplicationVM loanApplicationVM = (LoanApplicationVM)parameter;
            if (loanApplicationVM.Status != 1)
            {
                _canExecute = false;
                CanExecuteChanged.Invoke(this, new EventArgs());
                loanApplicationVM.Status = 1;
                Task.Run(() => Update(loanApplicationVM.InnerLoanApplication))
                    .ContinueWith(UpdateCallback, loanApplicationVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private LoanApplication Update(LoanApplication loanApplication)
        {
            using(ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ILoanApplicationService loanApplicationService = scope.Resolve<ILoanApplicationService>();
                return loanApplicationService.Update(settingsFactory.CreateLoanApi(), loanApplication).Result;
            }
        }

        private async Task UpdateCallback(Task<LoanApplication> update, object state)
        {
            try
            {
                LoanApplicationVM loanApplicationVM = (LoanApplicationVM)state;
                LoanApplication loanApplication = await update;
                loanApplicationVM.ApplicationStatusDescription = loanApplication.StatusDescription;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }
    }
}
