using BrassLoon.DataClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using BrassLoon.Interface.Authorization;
using BrassLoon.Interface.Authorization.Models;
using Microsoft.Data.SqlClient;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class UserReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[User]";
        private readonly IDataPurger _purger;
        private readonly ILogger<UserReporter> _logger;
        private readonly IUserService _userService;
        private readonly Settings _settings;
        
        public UserReporter(
            ISettingsFactory settingsFactory,
            Settings settings,
            IDbProviderFactory providerFactory,
            IDataPurger purger,
            ILogger<UserReporter> logger,
            IUserService userService)
            : base(settingsFactory, providerFactory)
        {
            _settings = settings;
            _purger = purger;
            _logger = logger;
            _userService = userService;
        }

        public int Order => 1;

        public async Task MergeWorkingDataToDestination()
        {
            using DbCommand command = (await GetDestinationConnection()).CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[lnwrk].[MergeUser]";
            command.CommandTimeout = 60;
            await command.ExecuteNonQueryAsync();
        }

        public async Task PurgeWorkingData()
        {
            await _purger.Purge(
                await GetDestinationConnection(),
                _workingTableName);
        }

        public async Task StageWorkingData()
        {
            _logger.LogInformation("Retreiving users");
            DataTable table = CreateTable();
            await foreach (User user in await _userService.GetByDomainId(SettingsFactory.CreateAuthorizationSettings(), _settings.AuthorizationDomainId.Value))
            {
                table.Rows.Add(
                    GetTableRow(table, user));
            }
            await StageWorkingData(table);
            _logger.LogInformation("Staged users");
        }

        private async Task StageWorkingData(DataTable table)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.TableLock;
            using SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)await GetDestinationConnection(), options, null);
            bulkCopy.DestinationTableName = _workingTableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        private static DataRow GetTableRow(DataTable table, User user)
        {
            DataRow row = table.NewRow();
            row["UserId"] = user.UserId.Value;
            row["Name"] = user.Name ?? string.Empty;
            row["EmailAddress"] = user.EmailAddress ?? string.Empty;
            return row;
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(Guid));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("EmailAddress", typeof(string));
            return table;
        }
    }
}
