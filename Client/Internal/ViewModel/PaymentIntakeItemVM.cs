using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Windows.Input;

namespace JCU.Internal.ViewModel
{
    public class PaymentIntakeItemVM : ViewModelBase
    {
        private readonly PaymentIntakeVM _paymentIntakeVM;
        private readonly PaymentIntake _paymentIntake;
        private ICommand _add;
        private ICommand _update;
        private ICommand _toggleHold;
        private string _loanNumber;
        private string _loanNumberTip;
        private DateTime? _nextPaymentDue;
        private bool _canAdd;

        public PaymentIntakeItemVM(PaymentIntakeVM paymentIntakeVM, PaymentIntake paymentIntake)
        {
            _paymentIntakeVM = paymentIntakeVM;
            _paymentIntake = paymentIntake;
            _loanNumber = paymentIntake.Loan?.Number ?? string.Empty;
            _nextPaymentDue = paymentIntake.Loan?.NextPaymentDue;
        }

        public PaymentIntake InnerPaymentIntake => _paymentIntake;
        public PaymentIntakeVM PaymentIntakeVM => _paymentIntakeVM;
        public Guid? PaymentIntakeId => _paymentIntake.PaymentIntakeId;
        public string CreateUserName => _paymentIntake.CreateUserName;
        public DateTime? CreateTimestamp => _paymentIntake.CreateTimestamp.HasValue ? _paymentIntake.CreateTimestamp.Value.ToLocalTime() : default(DateTime?);
        public string UpdateUserName
        {
            get => _paymentIntake.UpdateUserName;
            set
            {
                if (_paymentIntake.UpdateUserName != value)
                {
                    _paymentIntake.UpdateUserName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? UpdateTimestamp
        { 
            get => _paymentIntake.UpdateTimestamp.HasValue? _paymentIntake.UpdateTimestamp.Value.ToLocalTime() : default(DateTime?);
            set
            {
                if (_paymentIntake.UpdateTimestamp.HasValue != value.HasValue || (_paymentIntake.UpdateTimestamp.HasValue && _paymentIntake.UpdateTimestamp.Value != value.Value))
                {
                    _paymentIntake.UpdateTimestamp = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string StatusDescription
        {
            get => _paymentIntake.StatusDescription;
            set
            {
                if (_paymentIntake.StatusDescription != value)
                {
                    _paymentIntake.StatusDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Guid? LoanId
        {
            get => _paymentIntake.LoanId;
            set
            {
                if (_paymentIntake.LoanId.HasValue != value.HasValue || (_paymentIntake.LoanId.HasValue && _paymentIntake.LoanId.Value != value.Value))
                {
                    _paymentIntake.LoanId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? NextPaymentDue
        {
            get => _nextPaymentDue;
            set
            {
                if (_nextPaymentDue.HasValue != value.HasValue || (_nextPaymentDue.HasValue && _nextPaymentDue.Value != value.Value))
                {
                    _nextPaymentDue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CanAdd
        {
            get => _canAdd;
            set
            {
                if (_canAdd != value)
                {
                    _canAdd = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string LoanNumber
        {
            get => _loanNumber;
            set
            {
                if (_loanNumber != value)
                {
                    _loanNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string LoanNumberTip
        {
            get => _loanNumberTip;
            set
            {
                if (_loanNumberTip != value)
                {
                    _loanNumberTip = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand Add
        {
            get => _add;
            set
            {
                if (_add != value)
                {
                    _add = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand Update
        {
            get => _update;
            set
            {
                if (_update != value)
                {
                    _update = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand ToggleHold
        {
            get => _toggleHold;
            set
            {
                if (_toggleHold != value)
                {
                    _toggleHold = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? Date
        {
            get => _paymentIntake.Date;
            set
            {
                if (_paymentIntake.Date.HasValue != value.HasValue || (_paymentIntake.Date.HasValue && _paymentIntake.Date.Value != value.Value))
                {
                    _paymentIntake.Date = value.Value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? Amount
        {
            get => _paymentIntake.Amount;
            set
            {
                if (_paymentIntake.Amount.HasValue != value.HasValue || (_paymentIntake.Amount.HasValue && _paymentIntake.Amount.Value != value.Value))
                {
                    _paymentIntake.Amount = value.Value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string TransactionNumber
        {
            get => _paymentIntake.TransactionNumber;
            set
            {
                if (_paymentIntake.TransactionNumber != value)
                {
                    _paymentIntake.TransactionNumber = value ?? string.Empty; 
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
