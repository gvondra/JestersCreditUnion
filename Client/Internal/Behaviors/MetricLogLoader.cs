using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class MetricLogLoader
    {
        private readonly MetricLogVM _metricLogVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IMetricService _metricService;

        public MetricLogLoader(MetricLogVM metricLogVM,
            ISettingsFactory settingsFactory,
            IMetricService metricService)
        {
            _metricLogVM = metricLogVM;
            _settingsFactory = settingsFactory;
            _metricService = metricService;
            _metricLogVM.PropertyChanged += MetricLogVM_PropertyChanged;
        }

        private void MetricLogVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MetricLogVM.SelectedEventCode):
                    _metricLogVM.BusyVisibility = System.Windows.Visibility.Visible;
                    Task.Run(() => LoadItems(_metricLogVM.SelectedEventCode))
                        .ContinueWith(LoadItemsCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
            }
        }

        private Task<List<Metric>> LoadItems(string eventCode)
        {
            Task<List<Metric>> result = Task.FromResult<List<Metric>>(null);
            if (!string.IsNullOrEmpty(eventCode))
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _metricService.Search(settings, DateTime.UtcNow, eventCode);
            }
            return result;
        }

        private async Task LoadItemsCallback(Task<List<Metric>> loadItems, object state)
        {
            try
            {
                List<Metric> metrics = await loadItems;
                _metricLogVM.Items.Clear();
                if (metrics != null)
                {
                    foreach (Metric metric in metrics)
                    {
                        _metricLogVM.Items.Add(new MetricLogItemVM(metric));
                    }
                }
            }
            catch(System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _metricLogVM.BusyVisibility = System.Windows.Visibility.Collapsed;
            }
        }

        public void LoadEventCodes()
        {
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _metricService.GetEventCodes(settings);
            })
                .ContinueWith(LoadEventCodesCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadEventCodesCallback(Task<List<string>> loadEventCodes, object state)
        {
            try
            {
                _metricLogVM.SelectedEventCode = null;
                _metricLogVM.EventCodes.Clear();                
                foreach (string code in await loadEventCodes)
                {
                    _metricLogVM.EventCodes.Add(code);
                }
                if (_metricLogVM.EventCodes.Count > 0)
                    _metricLogVM.SelectedEventCode = _metricLogVM.EventCodes[0];
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
