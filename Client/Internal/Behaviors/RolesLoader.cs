using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class RolesLoader
    {
        private readonly RolesVM _rolesVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IRoleService _roleService;

        public RolesLoader(RolesVM rolesVM, ISettingsFactory settingsFactory, IRoleService roleService)
        {
            _rolesVM = rolesVM;
            _settingsFactory = settingsFactory;
            _roleService = roleService;
        }

        public void Load()
        {
            _rolesVM.BusyVisibility = System.Windows.Visibility.Visible;
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _roleService.Get(settings);
            }).ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<Role>> load, object state)
        {
            try
            {
                _rolesVM.Roles.Clear();
                foreach (Role role in await load)
                {
                    _rolesVM.Roles.Add(new RoleVM(role));
                }
                if (_rolesVM.Roles.Count == 0)
                {
                    _rolesVM.Roles.Add(new RoleVM());
                }
                _rolesVM.SelectedRole = _rolesVM.Roles[0];
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _rolesVM.BusyVisibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
