using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class WorkGroupTaskTypeVM : ViewModelBase
    {
        private readonly WorkTaskType _workTaskType;
        private bool _selected = false;

        private WorkGroupTaskTypeVM(WorkTaskType workTaskType)
        {
            _workTaskType = workTaskType;
        }

        public Guid WorkTaskTypeId => _workTaskType.WorkTaskTypeId.Value;

        public string Title => _workTaskType.Title;

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static WorkGroupTaskTypeVM Create(WorkTaskType workTaskType)
        {
            WorkGroupTaskTypeVM vm = new WorkGroupTaskTypeVM(workTaskType); 
            return vm;
        }
    }
}
