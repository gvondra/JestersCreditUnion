using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.ViewModel
{
    public class LoanVM : ViewModelBase
    {
        private readonly Loan _innerLoan;
        private readonly LoanAgreementVM _agreement;
        private Visibility _busyVisibility = Visibility.Collapsed;
        private Visibility _backVisibility = Visibility.Collapsed;
        private Visibility _disburseVisibility = Visibility.Collapsed;
        private Visibility _commandsVisibility = Visibility.Hidden;
        private ICommand _disburse;

        private LoanVM (Loan innerLoan)
        {
            _innerLoan = innerLoan;
            _agreement = LoanAgreementVM.Create(innerLoan.Agreement);
        }

        public LoanAgreementVM Agreement => _agreement;

        public Loan InnerLoan => _innerLoan;

        public string Number => _innerLoan.Number;

        public string StatusDescription => _innerLoan.StatusDescription ?? string.Empty;

        public DateTime? FirstPaymentDue
        {
            get => _innerLoan.FirstPaymentDue;
            set
            {
                if (_innerLoan.FirstPaymentDue.HasValue != value.HasValue 
                    || (_innerLoan.FirstPaymentDue.HasValue && _innerLoan.FirstPaymentDue.Value != value.Value))
                {
                    _innerLoan.FirstPaymentDue = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? NextPaymentDue
        {
            get => _innerLoan.NextPaymentDue;
            set
            {
                if (_innerLoan.NextPaymentDue.HasValue != value.HasValue
                    || (_innerLoan.NextPaymentDue.HasValue && _innerLoan.NextPaymentDue.Value != value.Value))
                {
                    _innerLoan.NextPaymentDue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand Disburse
        {
            get => _disburse;
            set
            {
                if (_disburse != value)
                {
                    _disburse = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility CommandsVisibility
        {
            get => _commandsVisibility;
            set
            {
                if (_commandsVisibility != value)
                {
                    _commandsVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility DisburseVisibility
        {
            get => _disburseVisibility;
            set
            {
                if (_disburseVisibility != value)
                {
                    _disburseVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility BackVisibility
        {
            get => _backVisibility;
            set
            {
                if (_backVisibility != value)
                {
                    _backVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Guid? LoanId
        {
            get => _innerLoan.LoanId;
            set
            {
                if (_innerLoan.LoanId.HasValue != value.HasValue 
                    || (_innerLoan.LoanId.HasValue && _innerLoan.LoanId.Value != value.Value))
                {
                    _innerLoan.LoanId = value;
                    NotifyPropertyChanged();
                }
            }
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

        public static LoanVM Create(Loan loan)
        {
            LoanVM vm = new LoanVM(loan);
            vm.DisburseVisibility = loan.FirstPaymentDue.HasValue ? Visibility.Collapsed : Visibility.Visible;
            vm.Disburse = new LoanDisburse();
            return vm;
        }
    }
}
