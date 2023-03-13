using System;

namespace JestersCreditUnion.Interface.Models
{
    public class Exception
    {
        public long? ExceptionId { get; set; }
        public string Message { get; set; }
        public string TypeName { get; set; }
        public string Source { get; set; }
        public string AppDomain { get; set; }
        public string TargetSite { get; set; }
        public string StackTrace { get; set; }
        public dynamic Data { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public Exception InnerException { get; set; }
    }
}
