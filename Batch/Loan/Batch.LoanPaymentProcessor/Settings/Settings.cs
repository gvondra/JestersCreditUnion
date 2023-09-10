using System;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public Guid? BrassLoonLogClientId { get; set; }
        public string BrassLoonLogClientSecret { get; set; }
        public Guid? LogDomainId { get; set; }
        public bool UseDefaultAzureSqlToken { get; set; }
    }
}
