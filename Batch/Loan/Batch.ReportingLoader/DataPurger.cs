using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class DataPurger : IDataPurger
    {
        private readonly ILogger<DataPurger> _logger;

        public DataPurger(ILogger<DataPurger> logger)
        {
            _logger = logger;
        }

        public async Task Purge(DbConnection connection, string tableName)
        {
            _logger.LogInformation($"Start purge {tableName}");
            using DbCommand command = connection.CreateCommand();
            command.CommandText = string.Format(CultureInfo.InvariantCulture, "TRUNCATE TABLE {0}", tableName);
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync();
            _logger.LogInformation($"Complete purge {tableName}");
        }
    }
}
