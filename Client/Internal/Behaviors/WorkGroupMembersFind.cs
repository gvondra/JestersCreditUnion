using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkGroupMembersFind : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IUserService _userService;
        private bool _canExecute = true;

        public WorkGroupMembersFind(ISettingsFactory settingsFactory,
            IUserService userService)
        {
            _settingsFactory = settingsFactory;
            _userService = userService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null) 
                throw new ArgumentNullException(nameof(parameter));
            if (typeof(WorkGroupVM).IsAssignableFrom(parameter.GetType()))
            {
                ExecuteFindWorkGroupMember((WorkGroupVM)parameter);
            }
        }

        private void ExecuteFindWorkGroupMember(WorkGroupVM workGroupVM)
        {
            workGroupVM.FoundMember = null;
            if (!string.IsNullOrEmpty(workGroupVM.FindMemberEmailAddress))
            {
                _canExecute = false;
                this.CanExecuteChanged.Invoke(this, new EventArgs());
                Task.Run(() =>
                {
                    ISettings settings = _settingsFactory.CreateApi();
                    return _userService.Search(settings, workGroupVM.FindMemberEmailAddress).Result;
                })
                    .ContinueWith(FindGroupMemberCallback, workGroupVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task FindGroupMemberCallback(Task<List<User>> find, object state)
        {
            try
            {
                List<User> users = await find;
                WorkGroupVM workGroupVM = (WorkGroupVM)state;
                if (users != null && users.Count > 0)
                {
                    workGroupVM.FoundMember = WorkGroupMemberVM.Create(users[0], workGroupVM);
                }
                else
                {
                    workGroupVM.FoundMember = null;
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                this.CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }
    }
}
