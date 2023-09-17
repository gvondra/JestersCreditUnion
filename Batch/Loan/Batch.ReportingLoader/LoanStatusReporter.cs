using BrassLoon.DataClient;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanStatusReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanStatus]";
        private readonly IDataPurger _purger;

        public LoanStatusReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
        }

        public async Task PurgeWorkingData()
        {
            await _purger.Purge(
                await GetConnection(),
                _workingTableName);
        }
    }
}
