using LiveCharts;
using System.Collections.Generic;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationSummaryVM : ViewModelBase
    {
        private SeriesCollection _series;
        private List<string> _xAxisLabels;

        public SeriesCollection Series
        {
            get => _series;
            set
            {
                if (_series != value)
                {
                    _series = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<string> XAxisLabels
        {
            get => _xAxisLabels;
            set
            {
                if (value != _xAxisLabels)
                {
                    _xAxisLabels = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
