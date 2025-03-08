using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data.Internal
{
    public class LoanApplicationDataFactory : ILoanApplicationDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;

        public LoanApplicationDataFactory(IDbProviderFactory dbProviderFactory)
        {
            _providerFactory = dbProviderFactory;
        }

        public Task<IEnumerable<LoanApplicationCloseData>> GetClosed(ISettings settings, DateTime minApplicationDate)
        {
            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(_providerFactory, "minApplicationDate", DbType.Date, DataUtil.GetParameterValue(minApplicationDate))
            ];
            return GetGenericDataFactory<LoanApplicationCloseData>().GetData(
                settings,
                _providerFactory,
                "[lnrpt].[GetLoanApplicationClose]",
                () => new LoanApplicationCloseData(),
                parameters);
        }

        public Task<IEnumerable<LoanApplicationCountData>> GetCounts(ISettings settings, DateTime minApplicationDate)
        {
            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(_providerFactory, "minApplicationDate", DbType.Date, DataUtil.GetParameterValue(minApplicationDate))
            ];
            return GetGenericDataFactory<LoanApplicationCountData>().GetData(
                settings,
                _providerFactory,
                "[lnrpt].[GetLoanApplicationCount]",
                () => new LoanApplicationCountData(),
                parameters);
        }

        private static GenericDataFactory<T> GetGenericDataFactory<T>() => new GenericDataFactory<T>();
    }
}
