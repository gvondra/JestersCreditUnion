using BrassLoon.DataClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
            using DbCommand command = (await GetDestinationConnection()).CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[lnwrk].[MergeLoanBalance]";
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
            _logger.LogInformation("Retreiving loan balances");
            DataTable table;
            Dictionary<Guid, List<LoanAgreement>> loanAgreementLookup = await GetLoanAgreements();
            using (DbDataReader reader = await OpenReader())
            {
                table = await PopulateTable(CreateTable(), reader, loanAgreementLookup);
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
            sql.Append("SELECT [lh].[LoanId], [lh].[UpdateTimestamp] [Timestamp], [lh].[Status], [lh].[Balance], [l].[Number], [lh].[InitialDisbursementDate], [lh].[FirstPaymentDue], [lh].[NextPaymentDue] ");
            sql.AppendFormat("FROM {0} [lh] ", _sourceTableName);
            sql.Append("INNER JOIN [ln].[Loan] [l] on [lh].[LoanId] = [l].[LoanId] ");
            DbCommand command = (await GetSourceConnection()).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            return await command.ExecuteReaderAsync();
        }

        private static async Task<DataTable> PopulateTable(DataTable table, DbDataReader reader, Dictionary<Guid, List<LoanAgreement>> loanAgreementLookup)
        {
            DataRow row;
            while (await reader.ReadAsync())
            {
                row = await PopulateRow(table.NewRow(), reader, loanAgreementLookup);
                if (row["AgreementHash"] != DBNull.Value)
                    table.Rows.Add(row);
            }
            return table;
        }

        private static async Task<DataRow> PopulateRow(DataRow row, DbDataReader reader, Dictionary<Guid, List<LoanAgreement>> loanAgreementLookup)
        {
            int ordinal;
            Guid loanId = await reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("LoanId"));
            DateTime updateTimestamp = await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("Timestamp"));
            row["Timestamp"] = updateTimestamp;
            row["Date"] = GetLocalDate(updateTimestamp);

            ordinal = reader.GetOrdinal("Balance");
            row["Balance"] = await reader.IsDBNullAsync(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<decimal>(ordinal);
            ordinal = reader.GetOrdinal("Status");
            row["LoanStatus"] = await reader.GetFieldValueAsync<short>(ordinal);
            ordinal = reader.GetOrdinal("Number");
            row["Number"] = await reader.GetFieldValueAsync<string>(ordinal);
            ordinal = reader.GetOrdinal("InitialDisbursementDate");
            row["InitialDisbursementDate"] = await reader.IsDBNullAsync(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<DateTime>(ordinal);
            ordinal = reader.GetOrdinal("FirstPaymentDue");
            row["FirstPaymentDue"] = await reader.IsDBNullAsync(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<DateTime>(ordinal);
            ordinal = reader.GetOrdinal("NextPaymentDue");
            row["NextPaymentDue"] = await reader.IsDBNullAsync(ordinal) ? DBNull.Value : await reader.GetFieldValueAsync<DateTime>(ordinal);

            if (loanAgreementLookup.ContainsKey(loanId))
            {
                LoanAgreement loanAgreement = FindLoanAgreement(loanAgreementLookup[loanId], updateTimestamp);
                row["AgreementHash"] = Hash.HashLoanAgreement(loanAgreement.CreateDate, loanAgreement.AgreementDate, loanAgreement.InterestRate, loanAgreement.PaymentAmount);
                row["AgreementCreateDate"] = loanAgreement.CreateDate;
                row["AgreementDate"] = loanAgreement.AgreementDate.HasValue ? loanAgreement.AgreementDate.Value : DBNull.Value;
                row["InterestRate"] = loanAgreement.InterestRate;
                row["PaymentAmount"] = loanAgreement.PaymentAmount;
            }

            return row;
        }

        private static LoanAgreement FindLoanAgreement(
            List<LoanAgreement> loanAgreements,
            DateTime timestamp)
        {
            return loanAgreements
                .Where(la => la.UpdateTimestamp < timestamp.AddMilliseconds(2500))
                .OrderByDescending(la => la.UpdateTimestamp)
                .First();
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
            table.Columns.Add("LoanStatus", typeof(short));
            table.Columns.Add("Number", typeof(string));
            table.Columns.Add("InitialDisbursementDate", typeof(DateTime));
            table.Columns.Add("FirstPaymentDue", typeof(DateTime));
            table.Columns.Add("NextPaymentDue", typeof(DateTime));
            table.Columns.Add("AgreementHash", typeof(byte[]));
            table.Columns.Add("AgreementCreateDate", typeof(DateTime));
            table.Columns.Add("AgreementDate", typeof(DateTime));
            table.Columns.Add("InterestRate", typeof(decimal));
            table.Columns.Add("PaymentAmount", typeof(decimal));
            return table;
        }

        private async Task<Dictionary<Guid, List<LoanAgreement>>> GetLoanAgreements()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT [lah].[LoanId], [lah].[CreateDate], [lah].[AgreementDate], [lah].[InterestRate], [lah].[PaymentAmount], [lah].[UpdateTimestamp] ");
            sql.Append("FROM [ln].[LoanAgreementHistory] [lah]; ");
            using DbCommand command = (await GetSourceConnection()).CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            using DbDataReader reader = await command.ExecuteReaderAsync();
            List<LoanAgreement> loanAgreements = new List<LoanAgreement>();
            while (await reader.ReadAsync())
            {
                loanAgreements.Add(
                    new LoanAgreement(
                        await reader.GetFieldValueAsync<Guid>(reader.GetOrdinal("LoanId")),
                        await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("CreateDate")),
                        await reader.IsDBNullAsync(reader.GetOrdinal("AgreementDate")) ? default(DateTime?) : await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("AgreementDate")),
                        await reader.GetFieldValueAsync<decimal>(reader.GetOrdinal("InterestRate")),
                        await reader.GetFieldValueAsync<decimal>(reader.GetOrdinal("PaymentAmount")),
                        await reader.GetFieldValueAsync<DateTime>(reader.GetOrdinal("UpdateTimestamp"))
                        ));
            }
            return loanAgreements
                .GroupBy(la => la.LoanId)
                .ToDictionary(
                    grp => grp.Key,
                    grp => grp.ToList());
        }

        internal struct LoanAgreement
        {
            internal LoanAgreement(
                Guid loanId,
                DateTime createDate,
                DateTime? agreementDate,
                decimal interestRate,
                decimal paymentAmount,
                DateTime updateTimestamp)
            {
                LoanId = loanId;
                CreateDate = createDate;
                AgreementDate = agreementDate;
                InterestRate = interestRate;
                PaymentAmount = paymentAmount;
                UpdateTimestamp = updateTimestamp;
            }

            internal Guid LoanId { get; private set; }
            internal DateTime CreateDate { get; private set; }
            internal DateTime? AgreementDate { get; private set; }
            internal decimal InterestRate { get; private set; }
            internal decimal PaymentAmount { get; private set; }
            internal DateTime UpdateTimestamp { get; private set; }
        }
    }
}
