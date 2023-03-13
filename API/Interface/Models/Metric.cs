using System;

namespace JestersCreditUnion.Interface.Models
{
    public class Metric
    {
        public long? MetricId { get; set; }
        public string EventCode { get; set; }
        public double? Magnitude { get; set; }
        public dynamic Data { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public string Status { get; set; }
        public string Requestor { get; set; }
        public string RequestorName { get; set; }
    }
}
