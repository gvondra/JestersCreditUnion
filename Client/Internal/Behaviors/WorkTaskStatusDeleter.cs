using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskStatusDeleter : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskStatusService _workTaskStatusService;
        private bool _canExecute = true;

        public WorkTaskStatusDeleter(ISettingsFactory settingsFactory, IWorkTaskStatusService workTaskStatusService)
        {
            _settingsFactory = settingsFactory;
            _workTaskStatusService = workTaskStatusService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(WorkTaskStatusVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Expecting {nameof(parameter)} of type {typeof(WorkTaskStatusVM).FullName}");
            _canExecute = false;
            CanExecuteChanged.Invoke(this, new EventArgs());
            WorkTaskStatusVM workTaskStatusVM = (WorkTaskStatusVM)parameter;
            if (!workTaskStatusVM.IsNew)
            {
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    _workTaskStatusService.Delete(settings, workTaskStatusVM.InnerWorkTaskStatus.WorkTaskTypeId.Value, workTaskStatusVM.InnerWorkTaskStatus.WorkTaskStatusId.Value);
                })
                    .ContinueWith(SaveCallback, workTaskStatusVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task SaveCallback(Task save, object state)
        {
            try
            {
                await save;
                _canExecute = true;
                CanExecuteChanged.Invoke(this, new EventArgs());
                WorkTaskStatusVM workTaskStatusVM = (WorkTaskStatusVM)state;
                int i = workTaskStatusVM.WorkTaskTypeVM.WorkTaskStatusesVM.Items.IndexOf(workTaskStatusVM);
                if (i >= 0)
                {
                    workTaskStatusVM.WorkTaskTypeVM.WorkTaskStatusesVM.Items.RemoveAt(i);
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
