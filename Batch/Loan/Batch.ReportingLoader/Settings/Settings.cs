using System;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public bool UseDefaultAzureToken { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string ApiBaseAddress { get; set; }
        public string LoanApiBaseAddress { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Guid? BrassLoonLogClientId { get; set; }
        public string BrassLoonLogClientSecret { get; set; }
        public Guid? LogDomainId { get; set; }
        public string LoanStatusLookupCode { get; set; }
    }
}
