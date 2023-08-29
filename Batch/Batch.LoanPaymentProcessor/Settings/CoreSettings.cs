using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class CoreSettings : ISettings
    {
        private readonly Settings _settings;

        public CoreSettings(Settings settings)
        {
            _settings = settings;
        }

        public string BrassLoonAccountApiBaseAddress => throw new NotImplementedException();

        public string BrassLoonConfigApiBaseAddress => throw new NotImplementedException();

        public string BrassLoonWorkTaskApiBaseAddress => throw new NotImplementedException();

        public Guid? BrassLoonClientId => _settings.BrassLoonLogClientId;

        public string BrassLoonClientSecret => _settings.BrassLoonLogClientSecret;

        public Guid? ConfigDomainId => throw new NotImplementedException();

        public Guid? WorkTaskDomainId => throw new NotImplementedException();

        public string WorkTaskConfigurationCode => throw new NotImplementedException();

        public bool UseDefaultAzureSqlToken => _settings.UseDefaultAzureSqlToken;

        public Task<string> GetConnetionString() => Task.FromResult(_settings.ConnectionString);

        public Func<Task<string>> GetDatabaseAccessToken() => null;
    }
}
