using System;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class Settings
    {
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public Guid? LogDomainId { get; set; }
        public string ServiceBusNamespace { get; set; }
        public string ServiceBusNewLoanAppQueue { get; set; }
        public double? RunHours { get; set; }
    }
}
