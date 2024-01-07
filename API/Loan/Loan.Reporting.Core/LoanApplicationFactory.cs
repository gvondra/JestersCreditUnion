using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Reporting;
using JestersCreditUnion.Loan.Reporting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class LoanApplicationFactory : ILoanApplicationFactory
    {
        private readonly ILoanApplicationDataFactory _dataFactory;

        public LoanApplicationFactory(ILoanApplicationDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public async Task<IEnumerable<LoanApplicationSummaryItem>> GetLoanApplicationSummary(Framework.ISettings settings, DateTime minAppliationDate)
        {
            return (await Task.WhenAll(
                GetApplications(settings, minAppliationDate),
                GetClosed(settings, minAppliationDate)))
                .SelectMany(i => i);
        }

        private async Task<IEnumerable<LoanApplicationSummaryItem>> GetApplications(Framework.ISettings settings, DateTime minAppliationDate)
        {
            return (await _dataFactory.GetCounts(new DataSettings(settings), minAppliationDate))
                .Select(data => new LoanApplicationSummaryItem
                {
                    Year = data.ApplicationYear,
                    Month = data.ApplicationMonth,
                    Description = "Application",
                    Count = data.Count
                });
        }

        private async Task<IEnumerable<LoanApplicationSummaryItem>> GetClosed(Framework.ISettings settings, DateTime minAppliationDate)
        {
            return (await _dataFactory.GetClosed(new DataSettings(settings), minAppliationDate))
                .Select(data => new LoanApplicationSummaryItem
                {
                    Year = data.ClosedYear,
                    Month = data.ClosedMonth,
                    Description = data.StatusDescription,
                    Count = data.Count
                });
        }
    }
}
