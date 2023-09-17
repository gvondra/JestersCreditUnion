﻿using BrassLoon.DataClient;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanReporter : ReporterBase, IReporter
    {
        private const string _workingTableName = "[lnwrk].[Loan]";
        private readonly IDataPurger _purger;

        public LoanReporter(
            ISettingsFactory settingsFactory,
            IDbProviderFactory providerFactory,
            IDataPurger purger)
            : base(settingsFactory, providerFactory)
        {
            _purger = purger;
        }

        public int Order => 1;

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