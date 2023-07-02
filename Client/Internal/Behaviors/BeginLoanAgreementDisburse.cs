using Autofac;
using JCU.Internal.NavigationPage;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class BeginLoanAgreementDisburse : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is BeginLoanAgreementVM beginLoanAgreementVM)
            {
                if (!beginLoanAgreementVM.HasErrors && !beginLoanAgreementVM.Loan.HasErrors && !beginLoanAgreementVM.Loan.Agreement.HasErrors)
                {
                    _canExecute = false;
                    CanExecuteChanged.Invoke(this, new EventArgs());
                    Task.Run(() => Disburse(beginLoanAgreementVM.Loan.LoanId.Value))
                        .ContinueWith(DisburseCallback, beginLoanAgreementVM, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private static Loan Disburse(Guid loanId)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateApi();
                ILoanService loanService = scope.Resolve<ILoanService>();
                return loanService.Disbursement(settings, loanId).Result;
            }
        }

        private async Task DisburseCallback(Task<Loan> disburse, object state)
        {
            try
            {
                await disburse;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
