using System;

namespace JCU.Internal.ViewModel
{
    public class OpenLoanSummaryItemVM : ViewModelBase
    {
        private string _number;
        private decimal _balance;
        private DateTime _nextPaymentDue;

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

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime NextPaymentDue
        {
            get => _nextPaymentDue;
            set
            {
                if (_nextPaymentDue != value)
                {
                    _nextPaymentDue = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
