using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class HomeVM : ViewModelBase
    {
        private WorkTasksHomeVM _workTasksHomeVM = new WorkTasksHomeVM();
        private Visibility _workTasksHomeVisibility = Visibility.Collapsed;

        private HomeVM() { }

        public Action LoadWorkTasks { get; set; }

        public WorkTasksHomeVM WorkTasksHomeVM
        {
            get => _workTasksHomeVM;
            set
            {
                if (_workTasksHomeVM != value)
                {
                    _workTasksHomeVM = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static HomeVM Create(ISettingsFactory settingsFactory,
            IWorkGroupService workGroupService,
            IUserService userService,
            IWorkTaskService workTaskService)
        {
            HomeVM vm = new HomeVM();
            WorkTasksHomeLoader loader = new WorkTasksHomeLoader(vm.WorkTasksHomeVM, settingsFactory, workGroupService, userService, workTaskService);
            vm.AddBehavior(loader);
            vm.LoadWorkTasks = loader.Load;
            return vm;
        }

        public Visibility WorkTasksHomeVisibility
        {
            get => _workTasksHomeVisibility;
            set
            {
                if (_workTasksHomeVisibility != value)
                {
                    _workTasksHomeVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
