using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System.Collections.ObjectModel;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class WorkGroupsVM : ViewModelBase
    {
        private readonly ObservableCollection<WorkGroupVM> _items = new ObservableCollection<WorkGroupVM>();
        private Visibility _busyVisibility = Visibility.Collapsed;

        public ObservableCollection<WorkGroupVM> Items => _items;

        public Visibility BusyVisibility
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static WorkGroupsVM Create(ISettingsFactory settingsFactory,
            IWorkGroupService workGroupService,
            IUserService userService)
        {
            WorkGroupsVM vm = new WorkGroupsVM();
            WorkGroupsLoader loader = new WorkGroupsLoader(vm, settingsFactory, workGroupService, userService);
            vm.AddBehavior(loader);
            loader.Load();
            return vm;
        }
    }
}
