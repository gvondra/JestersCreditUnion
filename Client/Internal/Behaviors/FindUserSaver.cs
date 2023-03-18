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
    public class FindUserSaver
    {
        private readonly FindUserVM _findUserVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IUserService _userService;

        public FindUserSaver(FindUserVM findUserVM, ISettingsFactory settingsFactory, IUserService userService)
        {
            _findUserVM = findUserVM;
            _settingsFactory = settingsFactory;
            _userService = userService;
        }

        public void Save()
        {
            if (_findUserVM.User != null)
            {
                Task.Run(() =>
                {
                    User user = _findUserVM.User;
                    user.Roles = _findUserVM.Roles
                    .Where(r => r.IsActive)
                    .Select(r => new AppliedRole { Name = r.Name, PolicyName= r.PolicyName })
                    .ToList();
                    ISettings settings = _settingsFactory.CreateApi();
                    return _userService.Update(settings, user);
                }).ContinueWith(SaveCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public async Task SaveCallback(Task<User> save, object state)
        {
            try
            {
                _findUserVM.User = await save;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
