using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskSave : ICommand
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
                Task.Run(() => Save((WorkTaskVM)parameter))
                    .ContinueWith(SaveCallback, parameter, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private WorkTask Save(WorkTaskVM workTaskVM)
        {
            using(ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                workTaskVM.WorkTaskStatusVM = workTaskVM?.WorkTaskTypeMV?.WorkTaskStatusesVM?.SelectedItem;
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                IWorkTaskService workTaskService = scope.Resolve<IWorkTaskService>();
                return workTaskService.Update(settingsFactory.CreateApi(), workTaskVM.InnerWorkTask).Result;
            }
        }

        private async Task SaveCallback(Task<WorkTask> save, object state)
        {
            try
            {
                await save;
                MessageBox.Show("Work task saved", "Work Task", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
