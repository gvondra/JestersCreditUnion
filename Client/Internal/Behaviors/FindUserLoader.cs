using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class FindUserLoader
    {
        private readonly FindUserVM _findUserVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public FindUserLoader(FindUserVM findUserVM, ISettingsFactory settingsFactory, IRoleService roleService, IUserService userService)
        {
            _findUserVM = findUserVM;
            _settingsFactory = settingsFactory;
            _roleService = roleService;
            _userService = userService;
        }

        public void LoadAllRoles()
        {
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _roleService.Get(settings);
            }).ContinueWith(LoadAllRolesCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadAllRolesCallback(Task<List<Role>> loadAllRoles, object state)
        {
            try
            {
                _findUserVM.AllRoles = await loadAllRoles;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }

        public void FindUser()
        {
            _findUserVM.User = null;
            if (!string.IsNullOrEmpty(_findUserVM.FindAddress))
            {
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    return _userService.Search(settings, _findUserVM.FindAddress);
                }).ContinueWith(FindUserCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
            }            
        }

        private async Task FindUserCallback(Task<List<User>> findUser, object state)
        {
            try
            {
                List<User> users = await findUser;
                if (users != null && users.Count == 1)
                {
                    _findUserVM.User = users[0];
                    _findUserVM.Roles.Clear();
                    foreach (Role role in _findUserVM.AllRoles)
                    {
                        FindUserRoleVM findUserRoleVM = new FindUserRoleVM(role);
                        findUserRoleVM.IsActive = _findUserVM.User.Roles.Any(r => string.Equals(r.PolicyName, role.PolicyName, StringComparison.OrdinalIgnoreCase));                            
                        _findUserVM.Roles.Add(findUserRoleVM);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
