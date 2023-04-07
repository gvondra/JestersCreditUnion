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
    public class WorkGroupLoader
    {
        private readonly WorkGroupVM _workGroupVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IUserService _userService;

        public WorkGroupLoader(WorkGroupVM workGroupVM, ISettingsFactory settingsFactory, IUserService userService)
        {
            _workGroupVM = workGroupVM;
            _settingsFactory = settingsFactory;
            _userService = userService;
        }

        public void LoadMembers()
        {
            _workGroupVM.Members.Clear();
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                List<User> users = new List<User>();
                foreach (string userId in _workGroupVM.MemberUserIds)
                {
                    User user = _userService.Get(settings, Guid.Parse(userId)).Result;
                    if (user != null) 
                        users.Add(user);
                }
                return users;
            })
                .ContinueWith(LoadMembersCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadMembersCallback(Task<List<User>> loadUsers, object state)
        {
            try
            {
                _workGroupVM.Members.Clear();
                foreach (User user in await loadUsers)
                {
                    _workGroupVM.Members.Add(WorkGroupMemberVM.Create(user, _workGroupVM));
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
