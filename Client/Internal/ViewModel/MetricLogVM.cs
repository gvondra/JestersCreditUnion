using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class MetricLogVM : ViewModelBase
    {
        private readonly ObservableCollection<string> _eventCodes = new ObservableCollection<string>();
        private readonly ObservableCollection<object> _items = new ObservableCollection<object>();
        private string _selectedEventCode;
        private Visibility _busyVisibility = Visibility.Collapsed;

        public ObservableCollection<string> EventCodes => _eventCodes;

        public ObservableCollection<object> Items => _items;

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

        public string SelectedEventCode
        {
            get => _selectedEventCode;
            set
            {
                if (_selectedEventCode != value)
                {
                    _selectedEventCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // factory method for MetricLogVM
        public static MetricLogVM Create(
            ISettingsFactory settingsFactory,
            IMetricService metricService)
        {
            MetricLogVM vm = new MetricLogVM();
            MetricLogLoader loader = new MetricLogLoader(vm, settingsFactory, metricService);
            vm.AddBehavior(loader);
            loader.LoadEventCodes();
            return vm;
        }
    }
}
