using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InterfaceModels = JestersCreditUnion.Interface.Models;

namespace JCU.Internal.ViewModel
{
    public class FindUserVM : ViewModelBase
    {
        private string _findAddress;
        private InterfaceModels.User _user;
        private List<InterfaceModels.Role> _allRoles;
        private ObservableCollection<FindUserRoleVM> _roles = new ObservableCollection<FindUserRoleVM>();

        public Action FindUser { get; set; }
        public Action Save { get; set; }

        // the email address to be searched for
        public string FindAddress
        {
            get => _findAddress;
            set
            {
                if (_findAddress != value)
                {
                    _findAddress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<FindUserRoleVM> Roles => _roles;

        public InterfaceModels.User User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyPropertyChanged();
            }
        }

        public List<InterfaceModels.Role> AllRoles
        {
            get => _allRoles;
            set
            {
                _allRoles = value;
                NotifyPropertyChanged();
            }
        }

        public static FindUserVM Create(ISettingsFactory settingsFactory, IRoleService roleService, IUserService userService)
        {
            FindUserVM vm = new FindUserVM();
            FindUserLoader loader = new FindUserLoader(vm, settingsFactory, roleService, userService);
            vm.AddBehavior(loader);
            vm.FindUser = loader.FindUser;
            FindUserSaver saver = new FindUserSaver(vm, settingsFactory, userService);
            vm.AddBehavior(saver);
            vm.Save = saver.Save;
            loader.LoadAllRoles();
            return vm;
        }
    }
}
