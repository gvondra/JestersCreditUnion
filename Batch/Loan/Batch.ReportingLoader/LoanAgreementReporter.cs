using BrassLoon.DataClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanAgreementReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanAgreement]";
        private const string _sourceTableName = "[ln].[LoanAgreementHistory]";
        private static readonly string[] _sourceColumns = new string[]
        {
            "LoanId",
            "CreateDate",
            "AgreementDate",
            "InterestRate",
            "PaymentAmount"
        };
        private readonly IDataPurger _purger;
        private readonly ILogger<LoanAgreementReporter> _logger;

        public LoanAgreementReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger,
            ILogger<LoanAgreementReporter> logger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
            _logger = logger;
        }

        public int Order => 1;

        public async Task MergeWorkingDataToDestination()
        {
            using DbCommand command = (await GetConnection()).CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[lnwrk].[MergeLoanAgreement]";
            command.CommandTimeout = 60;
            await command.ExecuteNonQueryAsync();
        }

        public async Task PurgeWorkingData()
        {
            await _purger.Purge(
                await GetConnection(),
                _workingTableName);
        }

        public async Task StageWorkingData()
        {
            _logger.LogInformation("Retreiving loan agreement history");
            DataTable table;
            using (DbDataReader reader = await OpenReader())
            {
                table = await PopulateTable(CreateTable(), reader);
            }
            await StageWorkingData(table);
            _logger.LogInformation("Staged loan agreement history");
        }

        private async Task StageWorkingData(DataTable table)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.TableLock;
            using SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)await GetConnection(), options, null);
            bulkCopy.DestinationTableName = _workingTableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        private async Task<DbDataReader> OpenReader()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(string.Join(",", _sourceColumns.Select(c => $"[{c}]")));
            sql.AppendFormat(" FROM {0};", _sourceTableName);
            DbCommand command = (await  GetConnection()).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            return await command.ExecuteReaderAsync();
        }

        private static void CalculateHash(DataRow row)
        {
            row["Hash"] = Hash.HashLoanAgreement(
                (DateTime)row["CreateDate"],
                row["AgreementDate"] == DBNull.Value ? default(DateTime?) : (DateTime)row["AgreementDate"],
                (decimal)row["InterestRate"],
                (decimal)row["PaymentAmount"]);
        }

        private static async Task<DataTable> PopulateTable(DataTable table, DbDataReader reader)
        {
            while (await reader.ReadAsync())
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < _sourceColumns.Length; i += 1)
                {
                    int ordinal = reader.GetOrdinal(_sourceColumns[i]);
                    object value = DBNull.Value;
                    if (!reader.IsDBNull(ordinal))
                        value = reader.GetValue(ordinal);
                    row[_sourceColumns[i]] = value;
                }
                table.Rows.Add(row);
                CalculateHash(row);
            }
            return table;
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("LoanId", typeof(Guid));
            table.Columns.Add("Hash", typeof(byte[]));
            table.Columns.Add("CreateDate", typeof(DateTime));
            table.Columns.Add("AgreementDate", typeof(DateTime));
            table.Columns.Add("InterestRate", typeof(decimal));
            table.Columns.Add("PaymentAmount", typeof(decimal));
            return table;
        }
    }
}
