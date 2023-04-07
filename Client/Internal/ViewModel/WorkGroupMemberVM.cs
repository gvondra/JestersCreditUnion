using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class WorkGroupMemberVM : ViewModelBase
    {
        private readonly WorkGroupVM _workGroupVM;
        private readonly User _innerUser;
        private WorkGroupMemberRemove _memberRemove;

        private WorkGroupMemberVM(User innerUser, WorkGroupVM workGroupVM)
        {
            _innerUser = innerUser;
            _workGroupVM = workGroupVM;
        }

        public WorkGroupVM WorkGroupVM => _workGroupVM;

        public Guid UserId => _innerUser.UserId.Value;

        public string Name => _innerUser.Name;

        public WorkGroupMemberRemove MemberRemove
        {
            get => _memberRemove;
            set
            {
                if (_memberRemove != value)
                {
                    _memberRemove = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static WorkGroupMemberVM Create(User user, WorkGroupVM workGroupVM)
        {
            WorkGroupMemberVM vm = new WorkGroupMemberVM(user, workGroupVM);
            vm.MemberRemove = new WorkGroupMemberRemove();
            return vm;
        }
    }
}
