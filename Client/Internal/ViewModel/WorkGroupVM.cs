using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JCU.Internal.ViewModel
{
    public class WorkGroupVM : ViewModelBase
    {
        private readonly WorkGroup _innerWorkGroup;
        private readonly ObservableCollection<WorkGroupMemberVM> _members = new ObservableCollection<WorkGroupMemberVM>();
        private WorkGroupSaver _saver;
        private string _findMemberEmailAddress;
        private WorkGroupMembersFind _membersFind;
        private WorkGroupMemberVM _foundMember;
        private WorkGroupMemberAdd _memberAdd;

        private WorkGroupVM(WorkGroup workGroup)
        {
            _innerWorkGroup = workGroup;
        }
        
        internal WorkGroup InnerWorkGroup => _innerWorkGroup;

        public WorkGroupMemberAdd MemberAdd
        {
            get => _memberAdd;
            set
            {
                if (_memberAdd != value)
                {
                    _memberAdd = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public WorkGroupMemberVM FoundMember
        {
            get => _foundMember;
            set
            {
                if (_foundMember != value)
                {
                    _foundMember = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string FindMemberEmailAddress
        {
            get => _findMemberEmailAddress;
            set
            {
                if (_findMemberEmailAddress != value)
                {
                    _findMemberEmailAddress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Action LoadMembers { get; private set; }

        public WorkGroupMembersFind MembersFind
        {
            get => _membersFind;
            set
            {
                _membersFind = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<WorkGroupMemberVM> Members => _members;

        public Guid? WorkGroupId
        {
            get => _innerWorkGroup.WorkGroupId;
            set
            {
                _innerWorkGroup.WorkGroupId = value;
                NotifyPropertyChanged();
            }
        }

        public WorkGroupSaver WorkGroupSave
        {
            get => _saver;
            set
            {
                _saver = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get => _innerWorkGroup.Title;
            set
            {
                if (_innerWorkGroup.Title != value)
                {
                    _innerWorkGroup.Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _innerWorkGroup.Description;
            set
            {
                if (_innerWorkGroup.Description != value)
                {
                    _innerWorkGroup.Description = value;
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

        public DateTime UpdateTimestamp
        {
            get => _innerWorkGroup.UpdateTimestamp?.ToLocalTime() ?? DateTime.Now;
            set
            {
                _innerWorkGroup.UpdateTimestamp = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime CreateTimestamp
        {
            get => _innerWorkGroup.CreateTimestamp?.ToLocalTime() ?? DateTime.Now;
            set
            {
                _innerWorkGroup.CreateTimestamp = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> MemberUserIds => _innerWorkGroup.MemberUserIds ?? new List<string>();

        public void AddMemberUserId(string userId)
        {
            if (_innerWorkGroup.MemberUserIds == null)
                _innerWorkGroup.MemberUserIds = new List<string>();
            if (!_innerWorkGroup.MemberUserIds.Any(id => string.Equals(id, userId, StringComparison.OrdinalIgnoreCase)))
                _innerWorkGroup.MemberUserIds.Add(userId);
        }

        public static WorkGroupVM Create(ISettingsFactory settingsFactory, 
            IWorkGroupService workGroupService,
            IUserService userService)
        {
            return Create(new WorkGroup
            {
                Title = "new group"
            },
            settingsFactory,
            workGroupService,
            userService);
        }

        public static WorkGroupVM Create(WorkGroup workGroup, 
            ISettingsFactory settingsFactory, 
            IWorkGroupService workGroupService,
            IUserService userService)
        {
            WorkGroupVM vm = new WorkGroupVM(workGroup);
            vm.WorkGroupSave = new WorkGroupSaver(settingsFactory, workGroupService);
            WorkGroupValidator validator = new WorkGroupValidator(vm);
            vm.AddBehavior(validator);
            WorkGroupLoader loader = new WorkGroupLoader(vm, settingsFactory, userService);
            vm.AddBehavior(loader);
            vm.LoadMembers = loader.LoadMembers;
            vm.MembersFind = new WorkGroupMembersFind(settingsFactory, userService);
            vm.MemberAdd = new WorkGroupMemberAdd();
            return vm;
        }
    }
}
