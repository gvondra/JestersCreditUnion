using JestersCreditUnion.Interface.Models;

namespace JCU.Internal.ViewModel
{
    public class FindUserRoleVM : ViewModelBase
    {
        private bool _isActive = false;
        private Role _innerRole;

        public FindUserRoleVM(Role role)
        {
            _innerRole = role;
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Name => _innerRole.Name;
        public string PolicyName => _innerRole?.PolicyName;
    }
}
