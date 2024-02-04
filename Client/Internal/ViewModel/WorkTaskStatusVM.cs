using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskStatusVM : ViewModelBase
    {
        private readonly WorkTaskStatus _innerWorkTaskStatus;
        private readonly WorkTaskTypeVM _workTaskTypeVM;
        private bool _isNew = false;
        private WorkTaskStatusSaver _saver;
        private WorkTaskStatusDeleter _deleter;

        private WorkTaskStatusVM(WorkTaskStatus taskStatus, WorkTaskTypeVM workTaskTypeVM)
        {
            _innerWorkTaskStatus = taskStatus;
            _workTaskTypeVM = workTaskTypeVM;
        }

        internal WorkTaskStatus InnerWorkTaskStatus => _innerWorkTaskStatus;

        public WorkTaskTypeVM WorkTaskTypeVM => _workTaskTypeVM;

        public WorkTaskStatusSaver WorkTaskStatusSaver
        {
            get => _saver;
            set
            {
                _saver = value;
                NotifyPropertyChanged();
            }
        }

        public WorkTaskStatusDeleter WorkTaskStatusDeleter
        {
            get => _deleter;
            set
            {
                _deleter = value;
                NotifyPropertyChanged();
            }
        }

        public Guid? WorkTaskStatusId 
        { 
            get => _innerWorkTaskStatus.WorkTaskStatusId; 
            set
            {
                _innerWorkTaskStatus.WorkTaskStatusId = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get => _innerWorkTaskStatus.Name;
            set
            {
                if (_innerWorkTaskStatus.Name != value)
                {
                    _innerWorkTaskStatus.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Code
        {
            get => _innerWorkTaskStatus.Code;
            set
            {
                if (_innerWorkTaskStatus.Code != value)
                {
                    _innerWorkTaskStatus.Code = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _innerWorkTaskStatus.Description;
            set
            {
                if (_innerWorkTaskStatus.Description != value)
                {
                    _innerWorkTaskStatus.Description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsDefaultStatus 
        { 
            get => _innerWorkTaskStatus.IsDefaultStatus ?? false;
            set
            {
                if (!_innerWorkTaskStatus.IsDefaultStatus.HasValue || _innerWorkTaskStatus.IsDefaultStatus.Value != value)
                {
                    _innerWorkTaskStatus.IsDefaultStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsClosedStatus 
        { 
            get => _innerWorkTaskStatus.IsClosedStatus ?? false;
            set
            {
                if (!_innerWorkTaskStatus.IsClosedStatus.HasValue || _innerWorkTaskStatus.IsClosedStatus.Value != value)
                {
                    _innerWorkTaskStatus.IsClosedStatus = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        public bool IsNew
        {
            get => _isNew;
            set
            {
                if (_isNew != value)
                {
                    _isNew = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CanDelete => !_isNew && WorkTaskCount == 0;

        public int WorkTaskCount => _innerWorkTaskStatus.WorkTaskCount ?? 0;

        public DateTime CreateTimestamp
        {
            get => _innerWorkTaskStatus.CreateTimestamp?.ToLocalTime() ?? DateTime.Now;
            set
            {
                _innerWorkTaskStatus.CreateTimestamp = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime UpdateTimestamp
        {
            get => _innerWorkTaskStatus.UpdateTimestamp?.ToLocalTime() ?? DateTime.Now;
            set
            {
                _innerWorkTaskStatus.UpdateTimestamp = value;
                NotifyPropertyChanged();
            }
        }

        public static WorkTaskStatusVM Create(WorkTaskTypeVM workTaskTypeVM, ISettingsFactory settingsFactory, IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskStatusVM vm = Create(
                new WorkTaskStatus
                {
                    WorkTaskTypeId = workTaskTypeVM.WorkTaskTypeId,
                    Code = "new_status_" + Math.Floor(DateTime.Now.Subtract(new DateTime(2000, 1, 1)).TotalSeconds),
                    Name = "New Status",
                    IsClosedStatus = false,
                    IsDefaultStatus = false
                },
                workTaskTypeVM,
                settingsFactory,
                workTaskStatusService
                );
            vm.IsNew = true;
            return vm;
        }

        public static WorkTaskStatusVM Create(WorkTaskStatus workTaskStatus, WorkTaskTypeVM workTaskTypeVM, ISettingsFactory settingsFactory, IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskStatusVM vm = new WorkTaskStatusVM(workTaskStatus, workTaskTypeVM);
            vm.WorkTaskStatusSaver = new WorkTaskStatusSaver(settingsFactory, workTaskStatusService);
            vm.WorkTaskStatusDeleter = new WorkTaskStatusDeleter(settingsFactory, workTaskStatusService);
            return vm;
        }
    }
}
