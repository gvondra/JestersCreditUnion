using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskVM : ViewModelBase
    {
        private readonly WorkTask _workTask;
        private WorkTaskTypeVM _workTaskTypeVM;
        private WorkTaskStatusVM _workTaskStatusVM;

        private WorkTaskVM(WorkTask workTask)
        {
            _workTask = workTask;
        }

        public WorkTask InnerWorkTask => _workTask;

        public WorkTaskTypeVM WorkTaskTypeMV
        {
            get => _workTaskTypeVM;
            set
            {
                if (_workTaskTypeVM != value)
                {
                    _workTaskTypeVM = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public WorkTaskStatusVM WorkTaskStatusVM
        {
            get => _workTaskStatusVM;
            set
            {
                if (_workTaskStatusVM != value)
                {
                    _workTaskStatusVM = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _workTask.Title;
            set
            {
                if (_workTask.Title != value)
                {
                    _workTask.Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Text
        {
            get => _workTask.Text;
            set
            {
                if (_workTask.Text != value)
                {
                    _workTask.Text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? AssignedDate
        {
            get => _workTask.AssignedDate;
            set
            {
                if (_workTask.AssignedDate.HasValue != value.HasValue 
                    || (value.HasValue && _workTask.AssignedDate.Value != value.Value))
                {
                    _workTask.AssignedDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static WorkTaskVM Create(WorkTask workTask,
            ISettingsFactory settingsFactory,
            IWorkTaskTypeService workTaskTypeService,
            IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskVM vm = new WorkTaskVM(workTask);
            vm.WorkTaskTypeMV = WorkTaskTypeVM.Create(workTask.WorkTaskType, settingsFactory, workTaskTypeService, workTaskStatusService);
            vm.WorkTaskStatusVM = WorkTaskStatusVM.Create(workTask.WorkTaskStatus, vm.WorkTaskTypeMV, settingsFactory, workTaskStatusService);
            return vm;
        }
    }
}
