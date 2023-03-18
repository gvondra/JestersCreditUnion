using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class MetricLogItemVM : ViewModelBase
    {
        private readonly Metric _innerItem;

        public MetricLogItemVM(Metric innerItem)
        {
            _innerItem = innerItem;
        }

        public DateTime? Timestamp => _innerItem.CreateTimestamp?.ToLocalTime();

        public double? Magnitude => _innerItem.Magnitude != null ? Math.Round((double)_innerItem.Magnitude, 3) : default(double?);

        public string Status => _innerItem.Status;

        public string RequestorName => _innerItem.RequestorName;
    }
}
