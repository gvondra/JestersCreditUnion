using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class TraceLogLoader
    {
        private readonly TraceLogVM _traceLogVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ITraceService _traceService;

        public TraceLogLoader(TraceLogVM traceLogVM, ISettingsFactory settingsFactory, ITraceService traceService)
        {
            _traceLogVM = traceLogVM;
            _settingsFactory = settingsFactory;
            _traceService = traceService;
            _traceLogVM.PropertyChanged += TraceLogVM_PropertyChanged;
        }

        private void TraceLogVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TraceLogVM.SelectedEventCode):
                    _traceLogVM.BusyVisibility = System.Windows.Visibility.Visible;
                    _traceLogVM.Items.Clear();
                    Task.Run(() => LoadItems(_traceLogVM.SelectedEventCode))
                        .ContinueWith(LoadItemsCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
            }
        }

        private List<Trace> LoadItems(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _traceService.Search(settings, DateTime.UtcNow, code).Result;
            }
            else
            {
                return null;
            }
        }

        private async Task LoadItemsCallback(Task<List<Trace>> loadItems, object state)
        {
            try
            {
                List<Trace> traces = await loadItems;
                _traceLogVM.Items.Clear();
                if (traces != null)
                {
                    foreach (Trace trace in traces)
                    {
                        _traceLogVM.Items.Add(new TraceLogItemVM(trace));
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _traceLogVM.BusyVisibility = System.Windows.Visibility.Collapsed;
            }
        }

        public void LoadEventCodes()
        {
            _traceLogVM.EventCodes.Clear();
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _traceService.GetEventCodes(settings);
            }).ContinueWith(LoadEventCodesCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadEventCodesCallback(Task<List<string>> loadEventCodes, object state)
        {
            try
            {
                _traceLogVM.EventCodes.Clear();
                foreach (string code in await loadEventCodes)
                {
                    _traceLogVM.EventCodes.Add(code);
                }
                if (_traceLogVM.EventCodes.Count > 0)
                {
                    _traceLogVM.SelectedEventCode = _traceLogVM.EventCodes[0];
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
