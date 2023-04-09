using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel.WorkTasksHome
{
    public class WorkTaskStatusVM : ViewModelBase
    {
        private readonly WorkTaskStatus _workTaskStatus;
        private readonly ObservableCollection<WorkTaskVM> _workTasks = new ObservableCollection<WorkTaskVM>();
        private bool _isExpanded;

        public WorkTaskStatusVM(WorkTaskStatus workTaskStatus)
        {
            _workTaskStatus = workTaskStatus;
        }

        public Guid WorkTaskStatusId => _workTaskStatus.WorkTaskStatusId.Value;

        public string Name => _workTaskStatus.Name;

        public ObservableCollection<WorkTaskVM> WorkTasks => _workTasks;

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
