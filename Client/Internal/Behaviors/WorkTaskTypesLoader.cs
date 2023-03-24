using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskTypesLoader
    {
        private readonly WorkTaskTypesVM _workTaskTypesVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskTypeService _workTaskTypeService;
        private readonly IWorkTaskStatusService _workTaskStatusService;

        public WorkTaskTypesLoader(WorkTaskTypesVM workTaskTypesVM, 
            ISettingsFactory settingsFactory, 
            IWorkTaskTypeService workTaskTypeService,
            IWorkTaskStatusService workTaskStatusService)
        {
            _workTaskTypesVM = workTaskTypesVM;
            _settingsFactory = settingsFactory;
            _workTaskTypeService = workTaskTypeService;
            _workTaskStatusService = workTaskStatusService;
        }

        public void Load()
        {
            _workTaskTypesVM.Items.Clear();
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _workTaskTypeService.GetAll(settings).Result;
            }).ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public async Task LoadCallback(Task<List<WorkTaskType>> load, object state)
        {
            try
            {
                _workTaskTypesVM.Items.Clear();
                foreach (WorkTaskType workTaskType in await load)
                {
                    _workTaskTypesVM.Items.Add(WorkTaskTypeVM.Create(workTaskType, _settingsFactory, _workTaskTypeService, _workTaskStatusService));
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
