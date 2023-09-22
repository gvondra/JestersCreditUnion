using BrassLoon.DataClient;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[Loan]";
        private const string _sourceTableName = "[ln].[LoanHistory]";
        private static readonly string[] _sourceColumns = new string[]
        {
            "LoanId",
            "Number",
            "InitialDisbursementDate",
            "FirstPaymentDue",
            "NextPaymentDue"
        };
        private readonly IDataPurger _purger;
        private readonly ILogger<LoanReporter> _logger;

        public LoanReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger,
            ILogger<LoanReporter> logger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
            _logger = logger;
        }

        public int Order => 1;

        public async Task MergeWorkingDataToDestination()
        {
            using DbCommand command = (await GetDestinationConnection()).CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[lnwrk].[MergeLoan]";
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
            _logger.LogInformation("Retreiving loan history");
            DataTable table;
            using (DbDataReader reader = await OpenReader())
            {
                table = await PopulateTable(CreateTable(), reader);
            }
            await StageWorkingData(table);
            _logger.LogInformation("Staged loan history");
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
            sql.Append("SELECT [l].[LoanId], [l].[Number], [lh].[InitialDisbursementDate], [lh].[FirstPaymentDue], [lh].[NextPaymentDue] ");
            sql.AppendFormat("FROM {0} [lh] ", _sourceTableName);
            sql.Append("INNER JOIN [ln].[Loan] [l] on [lh].[LoanId] = [l].[LoanId];");
            DbCommand command = (await GetSourceConnection()).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            return await command.ExecuteReaderAsync();
        }

        private static async Task<DataTable> PopulateTable(DataTable table, DbDataReader reader)
        {
            while (await reader.ReadAsync())
            {
                object[] values = new object[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i += 1)
                {
                    int ordinal = reader.GetOrdinal(table.Columns[i].ColumnName);
                    if (reader.IsDBNull(ordinal))
                        values[i] = DBNull.Value;
                    else
                        values[i] = reader.GetValue(ordinal);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("LoanId", typeof(Guid));
            table.Columns.Add("Number", typeof(string));
            table.Columns.Add("InitialDisbursementDate", typeof(DateTime));
            table.Columns.Add("FirstPaymentDue", typeof(DateTime));
            table.Columns.Add("NextPaymentDue", typeof(DateTime));
            return table;
        }
    }
}
