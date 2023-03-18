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
    public class RoleSaver
    {
        private readonly RolesVM _rolesVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IRoleService _roleService;

        public RoleSaver(RolesVM rolesVM, ISettingsFactory settingsFactory, IRoleService roleService)
        {
            _rolesVM = rolesVM;
            _settingsFactory = settingsFactory;
            _roleService = roleService;
        }

        public void Save(RoleVM roleVM)
        {
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                if (roleVM.IsNew)
                {
                    return _roleService.Create(settings, roleVM.InnerRole);
                }
                else
                {
                    return _roleService.Update(settings, roleVM.InnerRole);
                }
            }).ContinueWith(SaveCallback, roleVM, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SaveCallback(Task<Role> save, object state)
        {
            try
            {
                Role role = await save;
                RoleVM roleVM = (RoleVM)state;
                int index = _rolesVM.Roles.IndexOf(roleVM);
                roleVM = new RoleVM(role);
                _rolesVM.Roles[index] = roleVM;
                _rolesVM.SelectedRole = roleVM;
                MessageBox.Show("Complete", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
