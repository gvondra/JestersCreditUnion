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
    public class WorkTaskStatusesLoader : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskStatusService _workTaskStatusService;

        public WorkTaskStatusesLoader(ISettingsFactory settingsFactory,
            IWorkTaskStatusService workTaskStatusService)
        {
            _settingsFactory = settingsFactory;
            _workTaskStatusService = workTaskStatusService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(WorkTaskStatusesVM).IsAssignableFrom(parameter.GetType()))
                throw new ArgumentException($"Given {nameof(parameter)} must be of type {typeof(WorkTaskStatusesVM).FullName}");
            WorkTaskStatusesVM workTaskStatusesVM = (WorkTaskStatusesVM)parameter;
            workTaskStatusesVM.Items.Clear();
            if (!workTaskStatusesVM.WorkTaskTypeVM.IsNew)
            {
                Task.Run(() => Load(workTaskStatusesVM.WorkTaskTypeVM.WorkTaskTypeId.Value))
                    .ContinueWith(LoadCallback, workTaskStatusesVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private List<WorkTaskStatus> Load(Guid workTaskTypeId)
        {
            ISettings settings = _settingsFactory.CreateApi();
            return _workTaskStatusService.GetAll(settings, workTaskTypeId).Result;
        }

        private async Task LoadCallback(Task<List<WorkTaskStatus>> load, object state)
        {
            try
            {
                WorkTaskStatusesVM workTaskStatusesVM = (WorkTaskStatusesVM)state;
                workTaskStatusesVM.Items.Clear();
                foreach (WorkTaskStatus workTaskStatus in await load) 
                { 
                    workTaskStatusesVM.Items.Add(WorkTaskStatusVM.Create(workTaskStatus, _settingsFactory, _workTaskStatusService));
                }
                if (workTaskStatusesVM.Items.Count > 0)
                    workTaskStatusesVM.SelectedItem = workTaskStatusesVM.Items[0];
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
