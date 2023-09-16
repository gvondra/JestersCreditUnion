using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.ViewModel
{
    public class ReceiveLoanPaymentVM : ViewModelBase
    {
        private readonly LoanVM _loan;
        private readonly LoanPayment _loanPayment;
        private ICommand _save;
        private Visibility _busyVisibility = Visibility.Collapsed;
        private bool _saveEnabled = true;

        private ReceiveLoanPaymentVM(LoanVM loan)
        {
            _loan = loan;
            _loanPayment = new LoanPayment
            {
                LoanNumber = loan.Number,
                Date = DateTime.Today,
                TransactionNumber = string.Empty
            };
        }

        internal LoanPayment InnerLoanPayment => _loanPayment;

        public string Number => _loanPayment.LoanNumber;

        public DateTime ReceivedDate
        {
            get => _loanPayment.Date.Value.Date;
            set
            {
                if (_loanPayment.Date.Value != value)
                {
                    _loanPayment.Date = value.Date;
                    NotifyPropertyChanged();
                }
            }
        }

        public string TransactionNumber
        {
            get => _loanPayment.TransactionNumber;
            set
            {
                if (_loanPayment.TransactionNumber != value)
                {
                    _loanPayment.TransactionNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? Amount
        {
            get => _loanPayment.Amount;
            set
            {
                if (_loanPayment.Amount.HasValue != value.HasValue
                    || (_loanPayment.Amount.HasValue && _loanPayment.Amount.Value != value.Value))
                {
                    _loanPayment.Amount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Message
        {
            get => _loanPayment.Message;
            set
            {
                if (_loanPayment.Message != value)
                {
                    _loanPayment.Message = value;
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

        public ICommand Save
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool SaveEnabled
        {
            get => _saveEnabled;
            set
            {
                if (_saveEnabled != value)
                {
                    _saveEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static ReceiveLoanPaymentVM Create(LoanVM loan)
        {
            ReceiveLoanPaymentVM vm = new ReceiveLoanPaymentVM(loan)
            {
                Save = new ReceiveLoanPaymentSave()
            };
            vm.AddBehavior(new ReceiveLoanPaymentValidator(vm));
            return vm;
        }
    }
}
