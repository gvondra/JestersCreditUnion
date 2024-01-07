using System;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class Settings
    {
        public string SourceConnectionString { get; set; }
        public string DestinationConnectionString { get; set; }
        public bool UseDefaultAzureToken { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string ApiBaseAddress { get; set; }
        public string LoanApiBaseAddress { get; set; }
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string AuthorizationApiBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public Guid? AuthorizationDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? WorkTaskDomainId { get; set; }
        public string LoanStatusLookupCode { get; set; }
        public string LoanApplicationStatusLookupCode { get; set; }
    }
}
