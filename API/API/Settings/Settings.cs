﻿using System;

namespace API
{
    public class Settings
    {
        public string BrassLoonAccountApiBaseAddress { get; set; }
        public string BrassLoonConfigApiBaseAddress { get; set; }
        public string BrassLoonLogApiBaseAddress { get; set; }
        public string BrassLoonWorkTaskApiBaseAddress { get; set; }
        public Guid? BrassLoonLogClientId { get; set; }
        public string BrassLoonLogClientSecret { get; set; }
        public Guid? AuthorizationDomainId { get; set; }
        public Guid? ConfigDomainId { get; set; }
        public Guid? LogDomainId { get; set; }
        public Guid? WorkTaskDomainId { get; set; } 
        public string AuthorizationApiBaseAddress { get; set; }
        public string KeyVaultAddress { get; set; }
        public string DatabaseHost { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUser { get; set; }
        public string WorkTaskConfigurationCode { get; set; }
        public string LookupIndexConfigurationCode { get; set; }
    }
}
