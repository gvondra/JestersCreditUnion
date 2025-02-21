using System;

namespace JestersCreditUnion.Interface.Models
{
    public class Trace
    {
        public Guid? TraceId { get; set; }
        public string EventCode { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public EventId? EventId { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
    }

    public struct EventId
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
