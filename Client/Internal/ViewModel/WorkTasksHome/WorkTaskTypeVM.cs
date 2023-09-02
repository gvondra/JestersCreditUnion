using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel.WorkTasksHome
{
    public class WorkTaskTypeVM : ViewModelBase
    {
        private readonly WorkTaskType _workTaskType;
        private readonly ObservableCollection<WorkTaskStatusVM> _workTaskStatuses = new ObservableCollection<WorkTaskStatusVM>();
        private bool _isExpanded = true;

        public WorkTaskTypeVM(WorkTaskType workTaskType)
        {
            _workTaskType = workTaskType;
        }

        public Guid WorkTaskTypeId => _workTaskType.WorkTaskTypeId.Value;

        public string Title => _workTaskType.Title;

        public ObservableCollection<WorkTaskStatusVM> WorkTaskStatuses => _workTaskStatuses;

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
