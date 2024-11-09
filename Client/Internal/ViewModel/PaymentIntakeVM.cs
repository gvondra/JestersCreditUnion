using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JCU.Internal.ViewModel
{
    public class PaymentIntakeVM : ViewModelBase
    {
        private PaymentIntakeItemVM _newItem;
        private ICommand _load;
        
        public ObservableCollection<PaymentIntakeItemVM> Items { get; } = new ObservableCollection<PaymentIntakeItemVM>();

        public ICommand Load
        {
            get => _load;
            set
            {
                if (_load != value)
                {
                    _load = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public PaymentIntakeItemVM NewItem
        {
            get => _newItem;
            set
            {
                if (_newItem != value)
                {
                    _newItem = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
