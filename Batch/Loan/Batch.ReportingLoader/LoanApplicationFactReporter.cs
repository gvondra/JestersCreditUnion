using BrassLoon.DataClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanApplicationFactReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanApplicationFact]";
        private const string _sourceTableName = "[ln].[LoanApplication]";
        private readonly IDataPurger _purger;
        private readonly ILogger<LoanApplicationFactReporter> _logger;

        public LoanApplicationFactReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger,
            ILogger<LoanApplicationFactReporter> logger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
            _logger = logger;
        }

        public int Order => 2;

        public async Task MergeWorkingDataToDestination()
        {
            using DbCommand command = (await GetDestinationConnection()).CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[lnwrk].[MergeLoanApplicationFact]";
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
            _logger.LogInformation("Retreiving loan application fact data");
            DataTable table;
            using (DbDataReader reader = await OpenReader())
            {
                table = await PopulateTable(CreateTable(), reader);
            }
            await StageWorkingData(table);
            _logger.LogInformation("Staged loan application fact data");
        }

        private async Task StageWorkingData(DataTable table)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.TableLock;
            using SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)await GetDestinationConnection(), options, null);
            bulkCopy.DestinationTableName = _workingTableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        private static async Task<DataTable> PopulateTable(DataTable table, DbDataReader reader)
        {
            while (await reader.ReadAsync())
            {
                table.Rows.Add(
                    await PopulateRow(table.NewRow(), reader));
            }
            return table;
        }

        private static async Task<DataRow> PopulateRow(DataRow row, DbDataReader reader)
        {
            int ordinal;
            ordinal = reader.GetOrdinal("ApplicationDate");
            row["ApplicationDate"] = await reader.GetFieldValueAsync<DateTime>(ordinal);
            ordinal = reader.GetOrdinal("ClosedDate");
            row["ClosedDate"] = await reader.IsDBNullAsync(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<DateTime>(ordinal);
            ordinal = reader.GetOrdinal("Amount");
            row["Amount"] = await reader.GetFieldValueAsync<decimal>(ordinal);
            ordinal = reader.GetOrdinal("Status");
            row["Status"] = await reader.GetFieldValueAsync<short>(ordinal);
            ordinal = reader.GetOrdinal("UserId");
            row["UserId"] = await reader.IsDBNullAsync(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<Guid>(ordinal);
            
            return row;
        }

        private async Task<DbDataReader> OpenReader()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT [la].[ApplicationDate], [la].[ClosedDate], [la].[Amount], [la].[Status], [la].[UserId] ");
            sql.AppendFormat("FROM {0} [la] ", _sourceTableName);
            DbCommand command = (await GetSourceConnection()).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            return await command.ExecuteReaderAsync();
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ApplicationDate", typeof(DateTime));
            table.Columns.Add("ClosedDate", typeof(DateTime));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Status", typeof(short));
            table.Columns.Add("UserId", typeof(Guid));
            return table;
        }
    }
}
