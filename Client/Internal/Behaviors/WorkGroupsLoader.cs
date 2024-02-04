using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class WorkGroupsLoader
    {
        private readonly WorkGroupsVM _workGroupsVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkGroupService _workGroupService;
        private readonly IUserService _userService;
        private readonly IWorkTaskTypeService _workTaskTypeService;

        public WorkGroupsLoader(WorkGroupsVM workGroupsVM,
            ISettingsFactory settingsFactory,
            IWorkGroupService workGroupService,
            IUserService userService,
            IWorkTaskTypeService workTaskTypeService)
        {
            _workGroupsVM = workGroupsVM;
            _settingsFactory = settingsFactory;
            _workGroupService = workGroupService;
            _userService = userService;
            _workTaskTypeService = workTaskTypeService;
        }

        public void Load()
        {
            _workGroupsVM.BusyVisibility = System.Windows.Visibility.Visible;
            _workGroupsVM.Items.Clear();
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _workGroupService.GetAll(settings).Result;
            }).ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<WorkGroup>> load, object state)
        {
            try
            {
                _workGroupsVM.Items.Clear();
                foreach (WorkGroup workGroup in await load)
                {
                    _workGroupsVM.Items.Add(WorkGroupVM.Create(workGroup, _settingsFactory, _workGroupService, _userService, _workTaskTypeService));
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _workGroupsVM.BusyVisibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
