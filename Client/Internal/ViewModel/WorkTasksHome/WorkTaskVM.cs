using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
