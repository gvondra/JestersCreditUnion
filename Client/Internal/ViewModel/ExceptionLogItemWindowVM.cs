using System.Collections.Generic;

namespace JCU.Internal.ViewModel
{
    public class ExceptionLogItemWindowVM : ViewModelBase
    {
        private readonly ExceptionLogItemVM _logItem;
        private List<ExceptionLogItemVM> _innerItems;

        public ExceptionLogItemWindowVM(ExceptionLogItemVM logItem)
        {
            _logItem = logItem;
            if(_logItem.InnerException != null)
            {
                InnerItems = new List<ExceptionLogItemVM>();
                AddInnerItems(_logItem.InnerException, InnerItems);
            }
        }

        public ExceptionLogItemVM LogItem => _logItem;

        public List<ExceptionLogItemVM> InnerItems
        {
            get => _innerItems;
            set
            {
                _innerItems = value;
                NotifyPropertyChanged();
            }
        }

        private static void AddInnerItems(ExceptionLogItemVM logItem, List<ExceptionLogItemVM> items)
        {
            items.Add(logItem);
            if (logItem.InnerException != null)
                AddInnerItems(logItem.InnerException, items);
        }
    }
}
