using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationDenialSave : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(LoanApplicationDenialVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Parameter must be of type {typeof(LoanApplicationDenialVM).Name}");
            LoanApplicationDenialVM loanApplicationDenialVM = (LoanApplicationDenialVM)parameter;
            _canExecute = false;
            this.CanExecuteChanged.Invoke(this, new EventArgs());
            Task.Run(() => Save(loanApplicationDenialVM))
                .ContinueWith(SaveCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Save(LoanApplicationDenialVM loanApplicationDenialVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ILoanApplicationService loanApplicationService = scope.Resolve<ILoanApplicationService>();
                loanApplicationService.Deny(
                    settingsFactory.CreateLoanApi(), 
                    loanApplicationDenialVM.LoanApplicationId, 
                    loanApplicationDenialVM.InnerLoanApplicationDenial).Wait();
            }
        }

        private async Task SaveCallback(Task save, object state)
        {
            try
            {
                await save;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                this.CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }
    }
}
