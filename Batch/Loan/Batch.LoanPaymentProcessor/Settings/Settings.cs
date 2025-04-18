using System;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonConfigApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public string BrassLoonAddressApiBaseAddress { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public Guid? ConfigDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? WorkTaskDomainId { get; set; }
        public Guid? AddressDomainId { get; set; }
        public bool UseDefaultAzureSqlToken { get; set; }
        public string WorkTaskConfigurationCode { get; set; }
    }
}
