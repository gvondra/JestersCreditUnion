using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Reporting;
using JestersCreditUnion.Loan.Reporting.Data;
using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class LoanSummaryFactory : ILoanSummaryFactory
    {
        private readonly IOpenLoanSummaryDataFactory _openLoanSummaryDataFactory;

        public LoanSummaryFactory(IOpenLoanSummaryDataFactory openLoanSummaryDataFactory)
        {
            _openLoanSummaryDataFactory = openLoanSummaryDataFactory;
        }

        private static OpenLoanSummary Create(OpenLoanSummaryData data) => new OpenLoanSummary(data);

        public async Task<IEnumerable<IOpenLoanSummary>> Get(Framework.ISettings settings)
        {
            return (await _openLoanSummaryDataFactory.Get(new DataSettings(settings)))
                .Select<OpenLoanSummaryData, IOpenLoanSummary>(Create);
        }
    }
}
