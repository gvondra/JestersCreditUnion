using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonCore
{
    public class TransactionHandler : ITransactionHandler
    {
        private ISettings _settings;

        public TransactionHandler(ISettings settings)
        {
            _settings = settings;
        }

        public DbConnection Connection { get; set; }
        public BrassLoon.DataClient.IDbTransaction Transaction { get; set; }

        public Func<Task<string>> GetAccessToken => _settings.GetDatabaseAccessToken();

        public bool UseDefaultAzureToken => _settings.UseDefaultAzureSqlToken;

        public Task<string> GetConnectionString() => _settings.GetConnetionString();
    }
}
