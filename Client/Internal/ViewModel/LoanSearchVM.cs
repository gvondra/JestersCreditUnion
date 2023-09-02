using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JCU.Internal.ViewModel
{
    public class LoanSearchVM : ViewModelBase
    {
        private readonly ObservableCollection<LoanVM> _loans = new ObservableCollection<LoanVM>();
        private string _number;
        private string _borrowerName;
        private DateTime? _borrowerBirthDate;
        private ICommand _search;

        private LoanSearchVM() { }

        public ObservableCollection<LoanVM> Loans => _loans;

        public ICommand Search
        {
            get => _search;
            set
            {
                if (_search != value)
                {
                    _search = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Number
        {
            get => _number;
            set
            {
                if (_number != value)
                {
                    _number = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerName
        {
            get => _borrowerName ?? string.Empty;
            set
            {
                if (_borrowerName != value)
                {
                    _borrowerName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? BorrowerBirthDate
        {
            get => _borrowerBirthDate;
            set
            {
                if (_borrowerBirthDate.HasValue != value.HasValue
                    || (value.HasValue && _borrowerBirthDate.Value != value.Value))
                {
                    _borrowerBirthDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static LoanSearchVM Create()
        {
            LoanSearchVM vm = new LoanSearchVM();
            vm.Search = new Behaviors.LoanSearch();
            Behaviors.LoanSearchValidator loanSearchValidator = new Behaviors.LoanSearchValidator(vm);
            vm.AddBehavior(loanSearchValidator);
            return vm;
        }
    }
}
