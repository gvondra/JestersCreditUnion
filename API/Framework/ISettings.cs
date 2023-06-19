using System;

namespace JestersCreditUnion.Framework
{
    public interface ISettings : JestersCreditUnion.CommonCore.ISettings
    {
        public string BrassLoonAccountApiBaseAddress { get; }
        public string BrassLoonConfigApiBaseAddress { get; }
        public string BrassLoonWorkTaskApiBaseAddress { get; }
        public Guid? BrassLoonClientId { get; }
        public string BrassLoonClientSecret { get; }
        public Guid? ConfigDomainId { get; }
        public Guid? WorkTaskDomainId { get; }
        public string WorkTaskConfigurationCode { get; }
    }
}
