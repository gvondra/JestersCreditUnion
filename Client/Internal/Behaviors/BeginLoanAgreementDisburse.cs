using Autofac;
using JCU.Internal.Constants;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return loanService.InitiateDisbursement(settings, loanId).Result;
            }
        }

        private async Task DisburseCallback(Task<Loan> disburse, object state)
        {
            try
            {
                await disburse;
                BeginLoanAgreementVM beginLoanAgreementVM = (BeginLoanAgreementVM)state;

                _ = Task.Run(() => CloseLoanApplicationTasks(beginLoanAgreementVM.Loan.InnerLoan.LoanApplicationId.Value))
                    .ContinueWith(CloseLoanApplicationTasksCallback, beginLoanAgreementVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (System.Exception ex)
            {
                _canExecute = true;
                CanExecuteChanged?.Invoke(this, new EventArgs());
                ErrorWindow.Open(ex);
            }
        }

        private static void CloseLoanApplicationTasks(Guid loanApplicationId)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateApi();
                IWorkTaskService workTaskService = scope.Resolve<IWorkTaskService>();
                IWorkTaskStatusService workTaskStatusService = scope.Resolve<IWorkTaskStatusService>();
                List<WorkTask> worktasks = workTaskService.GetByContext(settings, WorkTaskContextTypes.LoanApplicationId, loanApplicationId.ToString("D"), false).Result;
                List<Dictionary<string, object>> patchData = new List<Dictionary<string, object>>();
                foreach (WorkTask workTask in worktasks.Where(wt => !(wt.WorkTaskStatus.IsClosedStatus ?? false)))
                {
                    List<WorkTaskStatus> statuses = workTaskStatusService.GetAll(settings, workTask.WorkTaskType.WorkTaskTypeId.Value).Result;
                    WorkTaskStatus status = statuses.FirstOrDefault(s => s.IsClosedStatus ?? false);
                    if (status != null)
                    {
                        patchData.Add(new Dictionary<string, object>
                        {
                            { "WorkTaskId", workTask.WorkTaskId.Value.ToString("D") },
                            { "WorkTaskStatusId", status.WorkTaskStatusId.Value.ToString("D") }
                        });
                    }
                }
                if (patchData.Count > 0)
                {
                    worktasks = workTaskService.Patch(settings, patchData).Result;
                }
            }
        }

        private async Task CloseLoanApplicationTasksCallback(Task closeLoanApplicationTasks, object state)
        {
            try
            {
                await closeLoanApplicationTasks;
                BeginLoanAgreementVM beginLoanAgreementVM = (BeginLoanAgreementVM)state;
                if (beginLoanAgreementVM?.NavigationService != null)
                {
                    beginLoanAgreementVM.NavigationService.Navigate(new Uri("NavigationPage/Home.xaml", UriKind.Relative));
                }
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
