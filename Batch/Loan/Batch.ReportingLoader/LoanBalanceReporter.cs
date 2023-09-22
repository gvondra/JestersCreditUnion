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
    public class LoanBalanceReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanBalance]";
        private const string _sourceTableName = "[ln].[LoanHistory]";
        private readonly IDataPurger _purger;
        private readonly ILogger<LoanBalanceReporter> _logger;

        public LoanBalanceReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger,
            ILogger<LoanBalanceReporter> logger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
            _logger = logger;
        }

        public int Order => 2;

        public async Task MergeWorkingDataToDestination()
        {

        }

        public async Task PurgeWorkingData()
        {
            await _purger.Purge(
                await GetDestinationConnection(),
                _workingTableName);
        }

        public async Task StageWorkingData()
        {
            _logger.LogInformation("Retreiving loan balances");
            DataTable table;
            using (DbDataReader reader = await OpenReader())
            {
                table = await PopulateTable(CreateTable(), reader);
            }
            await StageWorkingData(table);
            _logger.LogInformation("Staged loan balances");
        }

        private async Task StageWorkingData(DataTable table)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.TableLock;
            using SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)await GetDestinationConnection(), options, null);
            bulkCopy.DestinationTableName = _workingTableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        private async Task<DbDataReader> OpenReader()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT [lh].[UpdateTimestamp] [Timestamp], [lh].[Status], [lh].[Balance], [l].[LoanId], [l].[Number], [lh].[InitialDisbursementDate], [lh].[FirstPaymentDue], [lh].[NextPaymentDue] ");
            sql.AppendFormat("FROM {0} [lh] ", _sourceTableName);
            sql.Append("INNER JOIN [ln].[Loan] [l] on [lh].[LoanId] = [l].[LoanId] ");
            DbCommand command = (await GetSourceConnection()).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            return await command.ExecuteReaderAsync();
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
            ordinal = reader.GetOrdinal("Timestamp");
            row["Timestamp"] = await reader.GetFieldValueAsync<DateTime>(ordinal);
            row["Date"] = GetLocalDate((DateTime)row["Timestamp"]);
            ordinal = reader.GetOrdinal("Balance");
            row["Balance"] = reader.IsDBNull(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<decimal>(ordinal);
            ordinal = reader.GetOrdinal("Status");
            row["LoanStatus"] = await reader.GetFieldValueAsync<short>(ordinal);
            return row;
        }

        private static DateTime GetLocalDate(DateTime dateTimeUtc)
        {
            return TimeZoneInfo.ConvertTime(
                DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc),
                TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"))
                .Date;
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Timestamp", typeof(DateTime));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Balance", typeof(decimal));
            table.Columns.Add("LoanId", typeof(long));
            table.Columns.Add("LoanAgreementId", typeof(long));
            table.Columns.Add("LoanStatus", typeof(short));
            return table;
        }
    }
}
