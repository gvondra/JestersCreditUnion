using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace API
{
    public class Settings
    {
        private Guid? _authorizationDomainId;
        private Guid? _configDomainId;
        private Guid? _logDomainId;
        private Guid? _workTaskDomainId;

        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonConfigApiBaseAddress { get; set; }
        public string BrassLoonLogApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public Guid? BrassLoonLogClientId { get; set; }
        public string BrassLoonLogClientSecret { get; set; }
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
        public string BrassLoonAuthorizationApiBaseAddress { get; set; }
        public string WorkTaskConfigurationCode { get; set; }
        public string IdentitificationCardContainerName { get; set; }
        public string EncryptionKeyVault { get; set; }
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
