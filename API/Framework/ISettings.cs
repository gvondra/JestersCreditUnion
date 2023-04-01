using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ISettings
    {
        string DatabaseHost { get; }
        string DatabaseName { get; }
        string DatabaseUser { get; }
        Task<string> GetDatabasePassword();
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
