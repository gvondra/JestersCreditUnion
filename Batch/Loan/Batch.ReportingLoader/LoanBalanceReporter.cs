using BrassLoon.DataClient;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanBalanceReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanBalance]";
        private readonly IDataPurger _purger;

        public LoanBalanceReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
        }

        public int Order => 2;

        public async Task MergeWorkingDataToDestination()
        {

        }

        public async Task PurgeWorkingData()
        {
            await _purger.Purge(
                await GetConnection(),
                _workingTableName);
        }

        public async Task StageWorkingData()
        {
            
        }
    }
}
