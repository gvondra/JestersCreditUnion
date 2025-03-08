using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data.Internal
{
    public class OpenLoanSummaryDataFactory : IOpenLoanSummaryDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;

        public OpenLoanSummaryDataFactory(IDbProviderFactory dbProviderFactory)
        {
            _providerFactory = dbProviderFactory;
        }

        public Task<IEnumerable<OpenLoanSummaryData>> Get(ISettings settings)
        {
            return GetGenericDataFactory<OpenLoanSummaryData>().GetData(
                settings,
                _providerFactory,
                "[lnrpt].[GetOpenLoanSummary]",
                () => new OpenLoanSummaryData());
        }

        private static GenericDataFactory<T> GetGenericDataFactory<T>() => new GenericDataFactory<T>();
    }
}
