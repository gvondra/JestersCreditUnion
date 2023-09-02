using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;

namespace JCU.Internal.ViewModel
{
    public class RoleVM : ViewModelBase
    {
        private readonly Role _innerRole;
        public bool _isNew = false;

        public RoleVM(Role innerRole)
        {
            _innerRole = innerRole;
            AddBehavior(new RoleValidator(this));
        }

        public RoleVM() : this(new Role()) { }

        public Role InnerRole => _innerRole;

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

        public Guid? RoleId
        {
            get => _innerRole.RoleId;
            set
            {
                _innerRole.RoleId = value;
                NotifyPropertyChanged();
            }
        }

        public string PolicyName
        {
            get => _innerRole.PolicyName;
            set
            {
                if (_innerRole.PolicyName != value)
                {
                    _innerRole.PolicyName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _innerRole.Name;
            set
            {
                if (_innerRole.Name != value)
                {
                    _innerRole.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
