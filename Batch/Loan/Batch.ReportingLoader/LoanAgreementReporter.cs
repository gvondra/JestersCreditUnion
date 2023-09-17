using BrassLoon.DataClient;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanAgreementReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[LoanAgreement]";
        private readonly IDataPurger _purger;

        public LoanAgreementReporter(
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
