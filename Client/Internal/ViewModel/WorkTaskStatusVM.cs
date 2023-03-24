using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskStatusVM : ViewModelBase
    {
        private readonly WorkTaskStatus _innerWorkTaskStatus;
        private bool _isNew = false;
        private WorkTaskStatusSaver _saver;

        private WorkTaskStatusVM(WorkTaskStatus taskStatus)
        {
            _innerWorkTaskStatus = taskStatus;
        }

        internal WorkTaskStatus InnerWorkTaskStatus => _innerWorkTaskStatus;

        public WorkTaskStatusSaver WorkTaskStatusSaver
        {
            get => _saver;
            set
            {
                _saver = value;
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
                settingsFactory,
                workTaskStatusService
                );
            vm.IsNew = true;
            return vm;
        }

        public static WorkTaskStatusVM Create(WorkTaskStatus workTaskStatus, ISettingsFactory settingsFactory, IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskStatusVM vm = new WorkTaskStatusVM(workTaskStatus);
            vm.WorkTaskStatusSaver = new WorkTaskStatusSaver(settingsFactory, workTaskStatusService);
            return vm;
        }
    }
}
