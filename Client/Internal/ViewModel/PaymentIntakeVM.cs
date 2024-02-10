using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class PaymentIntakeVM : ViewModelBase
    {
        private PaymentIntakeItemVM _newItem;
        
        public ObservableCollection<PaymentIntakeItemVM> Items { get; } = new ObservableCollection<PaymentIntakeItemVM>();

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
