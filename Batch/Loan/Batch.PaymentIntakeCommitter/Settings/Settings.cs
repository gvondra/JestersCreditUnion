using System;

namespace JestersCreditUnion.Batch.PaymentIntakeCommitter
{
    public class Settings
    {
        public string LoanApiBaseAddress { get; set; }
        public string BrassLoonAuthorizationApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Guid? BrassLoonLogClientId { get; set; }
        public string BrassLoonLogClientSecret { get; set; }
        public Guid? AuthorizationDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
    }
}
