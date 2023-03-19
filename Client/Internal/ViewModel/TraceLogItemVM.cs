using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class TraceLogItemVM : ViewModelBase
    {
        private readonly Trace _innerTrace;

        public TraceLogItemVM(Trace innerTrace)
        {
            _innerTrace = innerTrace;
        }

        public string EventCode => _innerTrace.EventCode;

        public string Message => _innerTrace.Message;

        public DateTime? Timestamp => _innerTrace.CreateTimestamp?.ToLocalTime();

        public string EventName => _innerTrace.EventId?.Name ?? string.Empty;

        public string Category => _innerTrace.Category;

        public string Level => _innerTrace.Level;
    }
}
