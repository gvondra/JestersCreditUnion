using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel.WorkTasksHome
{
    public class WorkTaskVM : ViewModelBase
    {
        private readonly WorkTask _workTask;
        private bool _isExpanded;

        public WorkTaskVM(WorkTask workTask)
        {
            _workTask = workTask;
            WorkTaskClaim = new WorkTaskClaim();
        }

        public WorkTask InnerWorkTask => _workTask;

        public WorkTaskClaim WorkTaskClaim { get; set; }

        public Guid WorkTaskId => _workTask.WorkTaskId.Value;

        public string Title => _workTask.Title;

        public bool CanClaim => string.IsNullOrEmpty(_workTask.AssignedToUserId);

        public string AssignedToUserId
        {
            get => _workTask.AssignedToUserId;
            set
            {
                if (_workTask.AssignedToUserId != value)
                {
                    _workTask.AssignedToUserId = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(CanClaim));
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

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
