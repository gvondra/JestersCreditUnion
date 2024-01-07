using System;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace LoanAPI
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public bool EnableDatabaseAccessToken { get; set; }
        public string AuthorizationApiBaseAddress { get; set; }
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonConfigApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public string BrassLoonAddressApiBaseAddress { get; set; }
        public Guid? AuthorizationDomainId { get; set; }
        public Guid? ConfigDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? WorkTaskDomainId { get; set; }
        public Guid? AddressDomainId { get; set; }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public string WorkTaskConfigurationCode { get; set; }
        public string IdentitificationCardContainerName { get; set; }
        public string EncryptionKeyVault { get; set; }
        public string LookupIndexConfigurationCode { get; set; }
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure