using Autofac;
using JCU.Internal.ViewModel.WorkTasksHome;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskClaim : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null) 
                throw new ArgumentNullException(nameof(parameter));
            if (typeof(WorkTaskVM).IsAssignableFrom(parameter.GetType()))
            {
                _canExecute = false;
                this.CanExecuteChanged.Invoke(this, new EventArgs());
                Task.Run(() => AssignWorkTask((WorkTaskVM)parameter))
                    .ContinueWith(AssignWorkTaskCallback, parameter, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private ClaimWorkTaskResponse AssignWorkTask(WorkTaskVM workTaskVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateApi();
                IWorkTaskService workTaskService = scope.Resolve<IWorkTaskService>();
                IUserService userService = scope.Resolve<IUserService>();
                User user = userService.Get(settings).Result;
                return workTaskService.Claim(settings, workTaskVM.WorkTaskId, user.UserId.Value.ToString("D")).Result;
            }
        }

        private async Task AssignWorkTaskCallback(Task<ClaimWorkTaskResponse> assignWorkTask, object state)
        {
            try
            {
                ClaimWorkTaskResponse response = await assignWorkTask;
                WorkTaskVM workTaskVM = (WorkTaskVM)state;
                if (response.IsAssigned)
                {
                    workTaskVM.AssignedToUserId = response.AssignedToUserId; 
                }
                workTaskVM.AssignedDate = response.AssignedDate;
                MessageBox.Show(response.Message, "Claim Work Task", MessageBoxButton.OK, MessageBoxImage.Information);
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
