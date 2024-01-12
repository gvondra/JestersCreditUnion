using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkGroupSaver : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkGroupService _workGroupService;
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public WorkGroupSaver(ISettingsFactory settingsFactory,
            IWorkGroupService workGroupService)
        {
            _settingsFactory = settingsFactory;
            _workGroupService = workGroupService;
        }

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(WorkGroupVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Requires {nameof(parameter)} of type {typeof(WorkGroupVM).FullName}");
            _canExecute = false;
            this.CanExecuteChanged.Invoke(this, new EventArgs());
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                WorkGroupVM vm = (WorkGroupVM)parameter;
                WorkGroup workGroup = vm.InnerWorkGroup;
                workGroup.MemberUserIds = vm.Members.Select(m => m.UserId.ToString("D")).ToList();
                workGroup.WorkTaskTypeIds = vm.TaskTypes
                .Where(tt => tt.Selected)
                .Select(tt => tt.WorkTaskTypeId).ToList();
                if (workGroup.WorkGroupId.HasValue)
                    return _workGroupService.Update(settings, workGroup).Result;
                else
                    return _workGroupService.Create(settings, workGroup).Result;
            })
                .ContinueWith(SaveCallback, parameter, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SaveCallback(Task<WorkGroup> save, object state)
        {
            try
            {
                WorkGroup workGroup = await save;
                WorkGroupVM vm = (WorkGroupVM)state;
                vm.WorkGroupId = workGroup.WorkGroupId;
                vm.UpdateTimestamp = workGroup.UpdateTimestamp ?? DateTime.Now;
                vm.CreateTimestamp = workGroup.CreateTimestamp ?? DateTime.Now;
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
