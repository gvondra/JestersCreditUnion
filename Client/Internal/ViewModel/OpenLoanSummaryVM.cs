using System.Collections.Generic;

namespace JCU.Internal.ViewModel
{
    public class OpenLoanSummaryVM : ViewModelBase
    {
        private int _loanCount;
        private double _medianBalance;
        private int _count60DaysOverdue;
        private List<OpenLoanSummaryItemVM> _items;

        public int LoanCount
        {
            get => _loanCount;
            set
            {
                if (_loanCount != value)
                {
                    _loanCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double MedianBalance
        {
            get => _medianBalance;
            set
            {
                if (_medianBalance != value)
                {
                    _medianBalance = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Count60DaysOverdue
        {
            get => _count60DaysOverdue;
            set
            {
                if (_count60DaysOverdue != value)
                {
                    _count60DaysOverdue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<OpenLoanSummaryItemVM> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
