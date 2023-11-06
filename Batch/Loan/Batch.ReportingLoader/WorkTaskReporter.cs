using BrassLoon.DataClient;
using BrassLoon.Interface.WorkTask;
using BrassLoon.Interface.WorkTask.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class WorkTaskReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[WorkTask]";
        private readonly IDataPurger _purger;
        private readonly ILogger<UserReporter> _logger;
        private readonly ISettingsFactory _settingsFactory;
        private readonly Settings _settings;
        private readonly IWorkTaskService _workTaskService;

        public WorkTaskReporter(IDbProviderFactory providerFactory, IDataPurger purger, ILogger<UserReporter> logger, ISettingsFactory settingsFactory, Settings settings, IWorkTaskService workTaskService)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
            _logger = logger;
            _settingsFactory = settingsFactory;
            _settings = settings;
            _workTaskService = workTaskService;
        }

        // depends upon the user table
        public int Order => 2;

        public async Task MergeWorkingDataToDestination()
        {
            DbConnection connection = await GetDestinationConnection();
            await ExecuteNonQuery(connection, "[lnwrk].[MergeWorkTask]");
            await ExecuteNonQuery(connection, "[lnwrk].[MergeWorkTaskType]");
            await ExecuteNonQuery(connection, "[lnwrk].[MergeWorkTaskStatus]");
            await ExecuteNonQuery(connection, "[lnwrk].[MergeWorkTaskCycle]");
        }

        public async Task PurgeWorkingData()
        {
            await _purger.Purge(
                await GetDestinationConnection(),
                _workingTableName);
        }

        public async Task StageWorkingData()
        {
            _logger.LogInformation("Retreiving work tasks");
            DataTable table = null;
            await foreach (WorkTask workTask in await _workTaskService.GetAll(SettingsFactory.CreateWorkTaskSettings(), _settings.WorkTaskDomainId.Value))
            {
                if (table is null)
                    table = CreateTable();
                table.Rows.Add(
                    GetTableRow(table, workTask));
                if (table.Rows.Count >= 5000)
                {
                    await StageWorkingData(table);
                    table = null;
                }
            }
            if (table.Rows.Count > 0)
            {
                await StageWorkingData(table);
            }
            _logger.LogInformation("Staged work tasks");
        }

        private async Task StageWorkingData(DataTable table)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.TableLock;
            using SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)await GetDestinationConnection(), options, null);
            bulkCopy.DestinationTableName = _workingTableName;
            await bulkCopy.WriteToServerAsync(table);
        }

        private static DataRow GetTableRow(DataTable table, WorkTask workTask)
        {
            DataRow row = table.NewRow();
            row["WorkTaskId"] = workTask.WorkTaskId.Value;
            row["AssignedDate"] = workTask.AssignedDate.HasValue ? workTask.AssignedDate.Value : DBNull.Value;
            row["AssignedToUserId"] = !string.IsNullOrEmpty(workTask.AssignedToUserId) ? Guid.Parse(workTask.AssignedToUserId) : DBNull.Value;
            row["ClosedDate"] = workTask.ClosedDate.HasValue ? workTask.ClosedDate.Value : DBNull.Value;
            row["Title"] = workTask.Title ?? string.Empty;
            row["TypeCode"] = workTask.WorkTaskType.Code;
            row["TypeTitle"] = workTask.WorkTaskType.Title;
            row["StatusCode"] = workTask.WorkTaskStatus.Code;
            row["StatusName"] = workTask.WorkTaskStatus.Name;
            row["CreateTimestamp"] = workTask.CreateTimestamp.Value.ToLocalTime();
            return row;
        }

        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();            
            table.Columns.Add("WorkTaskId", typeof(Guid));
	        table.Columns.Add("AssignedDate", typeof(DateTime));
	        table.Columns.Add("AssignedToUserId", typeof(Guid));
	        table.Columns.Add("ClosedDate", typeof(DateTime));
	        table.Columns.Add("Title", typeof(string));
	        table.Columns.Add("TypeCode", typeof(string));
	        table.Columns.Add("TypeTitle", typeof(string));
	        table.Columns.Add("StatusCode", typeof(string));
	        table.Columns.Add("StatusName", typeof(string));
            table.Columns.Add("CreateTimestamp", typeof(DateTime));
            
            return table;
        }
    }
}
