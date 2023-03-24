using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskStatusSaver : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskStatusService _workTaskStatusService;
        private bool _canExecute = true;

        public WorkTaskStatusSaver(ISettingsFactory settingsFactory, IWorkTaskStatusService workTaskStatusService)
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
            if (workTaskStatusVM.IsNew)
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    return _workTaskStatusService.Create(settings, workTaskStatusVM.InnerWorkTaskStatus).Result;
                })
                    .ContinueWith(SaveCallback, workTaskStatusVM, TaskScheduler.FromCurrentSynchronizationContext());
            else
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    return _workTaskStatusService.Update(settings, workTaskStatusVM.InnerWorkTaskStatus).Result;
                })
                    .ContinueWith(SaveCallback, workTaskStatusVM, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SaveCallback(Task<WorkTaskStatus> save, object state)
        {
            try
            {
                WorkTaskStatus workTaskStatus = await save;
                WorkTaskStatusVM workTaskStatusVM = (WorkTaskStatusVM)state;
                workTaskStatusVM.IsNew = false;
                workTaskStatusVM.WorkTaskStatusId = workTaskStatus.WorkTaskStatusId;
                workTaskStatusVM.CreateTimestamp = workTaskStatus.CreateTimestamp.Value;
                workTaskStatusVM.UpdateTimestamp = workTaskStatus.UpdateTimestamp.Value;
                workTaskStatusVM.IsDefaultStatus = workTaskStatus.IsDefaultStatus ?? false;
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
