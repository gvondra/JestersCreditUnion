using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace API
{
    public class CoreSettings : ISettings
    {
        private readonly Settings _settings;

        public CoreSettings(Settings settings)
        {
            _settings = settings;
        }

        public string BrassLoonAccountApiBaseAddress => _settings.BrassLoonAccountApiBaseAddress;

        public string BrassLoonConfigApiBaseAddress => _settings.BrassLoonConfigApiBaseAddress;

        public string BrassLoonWorkTaskApiBaseAddress => _settings.BrassLoonWorkTaskApiBaseAddress;

        public Guid? BrassLoonClientId => _settings.BrassLoonLogClientId;

        public string BrassLoonClientSecret => _settings.BrassLoonLogClientSecret;

        public Guid? ConfigDomainId => _settings.ConfigDomainId;

        public Guid? WorkTaskDomainId => _settings.WorkTaskDomainId;

        public string WorkTaskConfigurationCode => _settings.WorkTaskConfigurationCode;

        public bool UseDefaultAzureSqlToken => _settings.EnableDatabaseAccessToken;

        public string IdentitificationCardContainerName => _settings.IdentitificationCardContainerName;

        public Task<string> GetConnetionString() => Task.FromResult(_settings.ConnectionString);

        public Func<Task<string>> GetDatabaseAccessToken() => null;
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
