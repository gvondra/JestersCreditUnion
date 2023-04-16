using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationVM : ViewModelBase
    {
        private readonly LoanApplication _loanApplication;
        private Visibility _busyVisibility = Visibility.Collapsed;

        private LoanApplicationVM(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public Visibility BusyVisibility 
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? ApplicationDate 
        { 
            get => _loanApplication.ApplicationDate;
            set
            {
                if (_loanApplication.ApplicationDate.HasValue != value.HasValue 
                    || (value.HasValue && _loanApplication.ApplicationDate.Value != value.Value))
                {
                    _loanApplication.ApplicationDate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string BorrowerName 
        { 
            get => _loanApplication.BorrowerName ?? string.Empty;
            set
            {
                if (_loanApplication.BorrowerName != value)
                {
                    _loanApplication.BorrowerName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? BorrowerBirthDate 
        { 
            get => _loanApplication.BorrowerBirthDate;
            set
            {
                if (_loanApplication.BorrowerBirthDate.HasValue != value.HasValue
                    || (value.HasValue && _loanApplication.BorrowerBirthDate.Value != value.Value))
                {
                    _loanApplication.BorrowerBirthDate = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        public static LoanApplicationVM Create(LoanApplication loanApplication)
        {
            LoanApplicationVM vm = new LoanApplicationVM(loanApplication);
            LoanApplicationValidator validator = new LoanApplicationValidator(vm);
            vm.AddBehavior(validator);
            return vm;
        }
    }
}
