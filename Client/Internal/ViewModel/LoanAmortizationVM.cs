using JCU.Internal.Behaviors;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class LoanAmortizationVM : ViewModelBase
    {
        private readonly LoanVM _loan;
        private readonly ObservableCollection<AmortizationItemVM> _amortizationItems = new ObservableCollection<AmortizationItemVM>();
        private Visibility _busyVisibility = Visibility.Collapsed;

        private LoanAmortizationVM(LoanVM loan)
        {
            _loan = loan;
        }

        public Guid LoanId => _loan.LoanId.Value;

        public ObservableCollection<AmortizationItemVM> AmortizationItems => _amortizationItems;

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

        public static LoanAmortizationVM Create(LoanVM loanVM)
        {
            LoanAmortizationVM vm = new LoanAmortizationVM(loanVM);
            LoanAmortizationLoader loader = new LoanAmortizationLoader(vm);
            vm.AddBehavior(loader);
            loader.Load();
            return vm;
        }
    }
}
