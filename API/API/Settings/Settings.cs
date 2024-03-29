﻿using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace API
{
    public class Settings
    {
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonConfigApiBaseAddress { get; set; }
        public string BrassLoonLogApiBaseAddress { get; set; }
        public string BrassLoonLogRpcBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public Guid? BrassLoonLogClientId { get; set; }
        public string BrassLoonLogClientSecret { get; set; }
        public Guid? AuthorizationDomainId { get; set; }
        public Guid? ConfigDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? WorkTaskDomainId { get; set; }
        public string BrassLoonAuthorizationApiBaseAddress { get; set; }
        public string WorkTaskConfigurationCode { get; set; }
        public string IdentitificationCardContainerName { get; set; }
        public string EncryptionKeyVault { get; set; }
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
