using BrassLoon.DataClient;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class DataSettings : ISettings
    {
        private readonly string _connectionString;

        public DataSettings(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<string> GetConnectionString() => Task.FromResult(_connectionString);
    }
}
