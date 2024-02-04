using System;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class Settings
    {
        public string BrassLoonAuthorizationApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string ApiBaseAddress { get; set; }
        public string LoanApiBaseAddress { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Guid? AuthorizationDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? WorkTaskDomainId { get; set; }
        public string ServiceBusNamespace { get; set; }
        public string ServiceBusNewLoanAppQueue { get; set; }
        public double? RunHours { get; set; }
    }
}
