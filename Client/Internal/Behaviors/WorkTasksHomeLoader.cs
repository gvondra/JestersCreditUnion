using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.Behaviors
{
    public class WorkTasksHomeLoader
    {
        private readonly WorkTasksHomeVM _workTasksHomeVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IUserService _userService;
        private readonly IWorkGroupService _workGroupService;
        private readonly IWorkTaskService _workTaskService;

        public WorkTasksHomeLoader(WorkTasksHomeVM workTasksHomeVM,
            ISettingsFactory settingsFactory,
            IWorkGroupService workGroupService,
            IUserService userService,
            IWorkTaskService workTaskService)
        {
            _workTasksHomeVM = workTasksHomeVM;
            _settingsFactory = settingsFactory;
            _workGroupService = workGroupService;
            _userService = userService;
            _workTaskService = workTaskService;
        }

        public void Load()
        {
            _workTasksHomeVM.BusyVisibility = Visibility.Visible;
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                User user = _userService.Get(settings).Result;
                IEnumerable<WorkTask> workTasks = new List<WorkTask>();
                foreach (WorkGroup workGroup in _workGroupService.GetByMemberUserId(settings, user.UserId.Value.ToString("D")).Result)
                {
                    workTasks = workTasks.Concat(
                        _workTaskService.GetByWorkGroupId(settings, workGroup.WorkGroupId.Value, false).Result
                        );
                }
                return workTasks
                .Where(wt => string.IsNullOrEmpty(wt.AssignedToUserId) || user.UserId.Value.Equals(Guid.Parse(wt.AssignedToUserId)))
                .Distinct(new WorkTaskComparer())
                .ToList();
            })
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<WorkTask>> load, object status)
        {
            try
            {
                foreach (WorkTask workTask in await load)
                {
                    ViewModel.WorkTasksHome.WorkTaskTypeVM workTaskTypeVM = _workTasksHomeVM.WorkTaskTypes.FirstOrDefault(wtt => wtt.WorkTaskTypeId.Equals(workTask.WorkTaskType.WorkTaskTypeId.Value));
                    if (workTaskTypeVM == null)
                    {
                        workTaskTypeVM = new ViewModel.WorkTasksHome.WorkTaskTypeVM(workTask.WorkTaskType);
                        _workTasksHomeVM.WorkTaskTypes.Add(workTaskTypeVM);
                    }
                    ViewModel.WorkTasksHome.WorkTaskStatusVM workTaskStatusVM = workTaskTypeVM.WorkTaskStatuses.FirstOrDefault(wts => wts.WorkTaskStatusId.Equals(workTask.WorkTaskStatus.WorkTaskStatusId.Value));
                    if (workTaskStatusVM == null)
                    {
                        workTaskStatusVM = new ViewModel.WorkTasksHome.WorkTaskStatusVM(workTask.WorkTaskStatus);
                        workTaskTypeVM.WorkTaskStatuses.Add(workTaskStatusVM);
                    }
                    workTaskStatusVM.WorkTasks.Add(new ViewModel.WorkTasksHome.WorkTaskVM(workTask));
                }
            }
            catch(System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _workTasksHomeVM.BusyVisibility = Visibility.Hidden;
            }
        }

        private class WorkTaskComparer : IEqualityComparer<WorkTask>
        {
            public bool Equals(WorkTask x, WorkTask y)
            {
                return x.WorkTaskId.Value.Equals(y.WorkTaskId.Value);
            }

            public int GetHashCode(WorkTask obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
