using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Reporting;
using JestersCreditUnion.Loan.Reporting.Data;
using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Core
{
    public class LoanBalanceFactory : ILoanBalanceFactory
    {
        private readonly ILoanBalanceDataFactory _dataFactory;

        public LoanBalanceFactory(ILoanBalanceDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public async Task<IEnumerable<LoanPastDue>> GetLoansPastDue(Framework.ISettings settings, short minDaysOverDue)
        {
            return (await _dataFactory.GetAll(new DataSettings(settings)))
                .GroupBy(bal => bal.Loan.Number)
                .Select(grp => grp.OrderDescending(new LoanBalanceDateComparer()).FirstOrDefault())
                .Where(bal => bal.DaysPastDue.HasValue && minDaysOverDue <= bal.DaysPastDue.Value)
                .Select(bal => new LoanPastDue { Number = bal.Loan.Number, DaysPastDue = bal.DaysPastDue.Value });
        }

        private sealed class LoanBalanceDateComparer : IComparer<LoanBalanceData>
        {
            public int Compare(LoanBalanceData x, LoanBalanceData y)
                => x.Date.CompareTo(y.Date);
        }
    }
}
