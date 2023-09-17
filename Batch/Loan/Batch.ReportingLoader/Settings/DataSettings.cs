using BrassLoon.DataClient;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class DataSettings : ISqlSettings
    {
        private readonly Settings _settings;

        public DataSettings(Settings settings)
        {
            _settings = settings;
        }

        public Func<Task<string>> GetAccessToken => null;

        public bool UseDefaultAzureToken => _settings.UseDefaultAzureToken;

        public Task<string> GetConnectionString() => Task.FromResult(_settings.ConnectionString);
    }
}
