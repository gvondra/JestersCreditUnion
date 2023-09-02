using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class TraceLogVM : ViewModelBase
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
        public static TraceLogVM Create(
            ISettingsFactory settingsFactory,
            ITraceService traceService)
        {
            TraceLogVM vm = new TraceLogVM();
            TraceLogLoader loader = new TraceLogLoader(vm, settingsFactory, traceService);
            vm.AddBehavior(loader);
            loader.LoadEventCodes();
            return vm;
        }
    }
}
