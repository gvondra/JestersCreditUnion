using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System.Collections.ObjectModel;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class RolesVM : ViewModelBase
    {
        private readonly ObservableCollection<RoleVM> _roles = new ObservableCollection<RoleVM>();
        private RoleVM _selectedRole;
        private Visibility _busyVisibility = Visibility.Collapsed;

        public ObservableCollection<RoleVM> Roles => _roles;

        public RoleVM SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                NotifyPropertyChanged();
            }
        }

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

        public static RolesVM Create(ISettingsFactory settingsFactory, IRoleService roleService)
        {
            RolesVM vm = new RolesVM();
            RolesLoader loader = new RolesLoader(vm, settingsFactory, roleService);
            vm.AddBehavior(loader);
            loader.Load();
            return vm;
        }
    }
}
