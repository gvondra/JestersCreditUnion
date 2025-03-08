using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data.Internal
{
    public class WorkTaskCycleSummaryDataFactory : IWorkTaskCycleSummaryDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly GenericDataFactory<WorkTaskCycleSummaryData> _genericDataFactory = new GenericDataFactory<WorkTaskCycleSummaryData>();

        public WorkTaskCycleSummaryDataFactory(IDbProviderFactory dbProviderFactory)
        {
            _providerFactory = dbProviderFactory;
        }

        public Task<IEnumerable<WorkTaskCycleSummaryData>> GetSummary(ISettings settings, DateTime minCreateDate)
        {
            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(_providerFactory, "minCreateDate", DbType.Date, DataUtil.GetParameterValue(minCreateDate))
            ];
            return _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[lnrpt].[GetWorkTaskCycleSummary]",
                () => new WorkTaskCycleSummaryData(),
                parameters);
        }
    }
}
