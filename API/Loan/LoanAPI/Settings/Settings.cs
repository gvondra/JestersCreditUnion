using System;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace LoanAPI
{
    public class Settings
    {
        private Guid? _authorizationDomainId;
        private Guid? _configDomainId;
        private Guid? _logDomainId;
        private Guid? _workTaskDomainId;
        private Guid? _addressDomainId;

        public string ConnectionString { get; set; }
        public bool EnableDatabaseAccessToken { get; set; }
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonAuthorizationApiBaseAddress { get; set; }
        public string BrassLoonConfigApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public string BrassLoonAddressApiBaseAddress { get; set; }
        public string ApiBaseAddress { get; set; }
        public Guid? BrassLoonDomainId { get; set; }
        public Guid? AuthorizationDomainId
        {
            get => _authorizationDomainId ?? BrassLoonDomainId;
            set => _authorizationDomainId = value;
        }
        public Guid? ConfigDomainId
        {
            get => _configDomainId ?? BrassLoonDomainId;
            set => _configDomainId = value;
        }
        public Guid? LogDomainId
        {
            get => _logDomainId ?? BrassLoonDomainId;
            set => _logDomainId = value;
        }
        public Guid? WorkTaskDomainId
        {
            get => _workTaskDomainId ?? BrassLoonDomainId;
            set => _workTaskDomainId = value;
        }
        public Guid? AddressDomainId
        {
            get => _addressDomainId ?? BrassLoonDomainId;
            set => _addressDomainId = value;
        }
        public Guid? BrassLoonClientId { get; set; }
        public string BrassLoonClientSecret { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string WorkTaskConfigurationCode { get; set; }
        public string InterestRateConfigurationCode { get; set; }
        public string IdentitificationCardContainerName { get; set; }
        public string EncryptionKeyVault { get; set; }
        public string LookupIndexConfigurationCode { get; set; }
        public string ServiceBusNamespace { get; set; }
        public string ServiceBusNewLoanAppQueue { get; set; }
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure