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
    public class WorkTaskTypeVM : ViewModelBase
    {
        private readonly WorkTaskType _innerWorkTaskType;
        private bool _isNew = false;
        private WorkTaskTypeSaver _workTaskTypeSave;
        private WorkTaskStatusesVM _workTaskStatusesVM;

        private WorkTaskTypeVM(WorkTaskType innerWorkTaskType)
        {
            _innerWorkTaskType = innerWorkTaskType;            
        }

        public WorkTaskStatusesVM WorkTaskStatusesVM
        {
            get => _workTaskStatusesVM;
            private set => _workTaskStatusesVM = value;
        }

        public WorkTaskType InnerWorkTaskType => _innerWorkTaskType;

        public WorkTaskTypeSaver WorkTaskTypeSave
        {
            get => _workTaskTypeSave;
            set
            {
                _workTaskTypeSave = value;
                NotifyPropertyChanged();
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

        public Guid? WorkTaskTypeId
        {
            get => _innerWorkTaskType.WorkTaskTypeId;
            set
            {
                _innerWorkTaskType.WorkTaskTypeId = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get => _innerWorkTaskType.Title;
            set
            {
                if (_innerWorkTaskType.Title != value)
                {
                    _innerWorkTaskType.Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Code
        {
            get => _innerWorkTaskType.Code;
            set
            {
                if (_innerWorkTaskType.Code != value)
                {
                    _innerWorkTaskType.Code = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _innerWorkTaskType.Description;
            set
            {
                if (_innerWorkTaskType.Description != value)
                {
                    _innerWorkTaskType.Description = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(DescriptionLine1));
                }
            }
        }

        public string DescriptionLine1
        {
            get
            {
                return (Description ?? string.Empty).Split('\n')[0].Trim();
            }
        }

        public int WorkTaskCount => _innerWorkTaskType.WorkTaskCount ?? 0;
        public DateTime CreateTimestamp
        {
            get => _innerWorkTaskType.CreateTimestamp?.ToLocalTime() ?? DateTime.Now;
            set
            {
                _innerWorkTaskType.CreateTimestamp = value;
                NotifyPropertyChanged();
            }
        }
        
        public DateTime UpdateTimestamp
        {
            get => _innerWorkTaskType.UpdateTimestamp?.ToLocalTime() ?? DateTime.Now;
            set
            {
                _innerWorkTaskType.UpdateTimestamp = value;
                NotifyPropertyChanged();
            }
        }
        

        public static WorkTaskTypeVM Create(ISettingsFactory settingsFactory, 
            IWorkTaskTypeService workTaskTypeService,
            IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskTypeVM vm = Create(new WorkTaskType()
            {
                Code = "new_type_" + Math.Floor(DateTime.Now.Subtract(new DateTime(2000, 1, 1)).TotalSeconds),
                Title = "new type"
            }, 
            settingsFactory, 
            workTaskTypeService,
            workTaskStatusService);
            vm.IsNew = true;
            return vm;
        }
        public static WorkTaskTypeVM Create(WorkTaskType workTaskType, 
            ISettingsFactory settingsFactory, 
            IWorkTaskTypeService workTaskTypeService,
            IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskTypeVM vm = new WorkTaskTypeVM(workTaskType);
            vm.WorkTaskTypeSave = new WorkTaskTypeSaver(settingsFactory, workTaskTypeService);
            WorkTaskTypeValidator validator = new WorkTaskTypeValidator(vm);
            vm.AddBehavior(validator);
            vm.WorkTaskStatusesVM = WorkTaskStatusesVM.Create(vm, settingsFactory, workTaskStatusService);
            return vm;
        }
    }
}
