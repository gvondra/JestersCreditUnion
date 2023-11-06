using BrassLoon.DataClient;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class DataSettings : ISqlSettings
    {
        private readonly string _connectionString;
        private readonly bool _useDefaultAzureToken;

        public DataSettings(string connectionString, bool useDefaultAzureToken)
        {
            _connectionString = connectionString;
            _useDefaultAzureToken = useDefaultAzureToken;
        }

        public Func<Task<string>> GetAccessToken => null;

        public bool UseDefaultAzureToken => _useDefaultAzureToken;

        public Task<string> GetConnectionString() => Task.FromResult(_connectionString);
    }
}
