using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskTypeSaver : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskTypeService _workTaskTypeService;
        private bool _canExecute = true;

        public WorkTaskTypeSaver(ISettingsFactory settingsFactory,
            IWorkTaskTypeService workTaskTypeService)
        {
            _settingsFactory = settingsFactory;
            _workTaskTypeService = workTaskTypeService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            _canExecute = false;
            this.CanExecuteChanged.Invoke(this, new EventArgs());
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(WorkTaskTypeVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Requires parameter of type {typeof(WorkTaskTypeVM).Name}");
            WorkTaskTypeVM workTaskTypeVM = (WorkTaskTypeVM)parameter;
            if (workTaskTypeVM.IsNew)
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    return _workTaskTypeService.Create(settings, workTaskTypeVM.InnerWorkTaskType).Result;
                }).ContinueWith(SaveCallback, workTaskTypeVM, TaskScheduler.FromCurrentSynchronizationContext());
            else
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    return _workTaskTypeService.Update(settings, workTaskTypeVM.InnerWorkTaskType).Result;
                }).ContinueWith(SaveCallback, workTaskTypeVM, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SaveCallback(Task<WorkTaskType> save, object state)
        {
            try
            {
                WorkTaskType workTaskType = await save;
                WorkTaskTypeVM workTaskTypeVM = (WorkTaskTypeVM)state;
                workTaskTypeVM.IsNew = false;
                workTaskTypeVM.WorkTaskTypeId = workTaskType.WorkTaskTypeId;
                workTaskTypeVM.CreateTimestamp = workTaskType.CreateTimestamp ?? DateTime.UtcNow;
                workTaskTypeVM.UpdateTimestamp = workTaskType.UpdateTimestamp ?? DateTime.UtcNow;
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
