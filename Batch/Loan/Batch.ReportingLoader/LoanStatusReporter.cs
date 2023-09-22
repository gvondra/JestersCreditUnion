using BrassLoon.DataClient;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanStatusReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanStatus]";
        private readonly IDataPurger _purger;
        private readonly ILogger<LoanStatusReporter> _logger;
        private readonly Settings _settings;
        private readonly ILookupService _lookupService;

        public LoanStatusReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger,
            ILogger<LoanStatusReporter> logger,
            Settings settings,
            ILookupService lookupService)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
            _logger = logger;
            _settings = settings;
            _lookupService = lookupService;
        }

        public int Order => 1;

        public async Task MergeWorkingDataToDestination()
        {
            using DbCommand command = (await GetDestinationConnection()).CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[lnwrk].[MergeLoanStatus]";
            command.CommandTimeout = 60;
            await command.ExecuteNonQueryAsync();
        }

        public async Task PurgeWorkingData()
        {
            _logger.LogInformation("Merging loan statuses to destination");
            await _purger.Purge(
                await GetDestinationConnection(),
                _workingTableName);
            _logger.LogInformation("Merged loan statuses to destination");
        }

        public async Task StageWorkingData()
        {
            _logger.LogInformation("Retreiving loan statuses");
            await StageWorkingData(
                PopulateTable(
                    CreateTable(),
                    await _lookupService.Get(SettingsFactory.CreateLoanApiSettings(), _settings.LoanStatusLookupCode)));
            _logger.LogInformation("Staged loan statuses");
        }

        private async Task StageWorkingData(DataTable table)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.TableLock;            
            using SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)await GetDestinationConnection(), options, null);
            bulkCopy.DestinationTableName = _workingTableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        private static DataTable PopulateTable(DataTable table, Lookup lookup)
        {
            foreach (KeyValuePair<string, string> pair in lookup.Data)
            {
                table.Rows.Add(values: new object[] { pair.Key, pair.Value });
            }
            return table;
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Status", typeof(short));
            table.Columns.Add("Description", typeof(string));
            return table;
        }
    }
}
