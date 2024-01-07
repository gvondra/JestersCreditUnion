using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationSummaryLoader
    {
        private readonly LoanApplicationSummaryVM _loanApplicationSummary;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanApplicationSummary _service;

        public LoanApplicationSummaryLoader(
            LoanApplicationSummaryVM loanApplicationSummary,
            ISettingsFactory settingsFactory,
            ILoanApplicationSummary service)
        {
            _loanApplicationSummary = loanApplicationSummary;
            _settingsFactory = settingsFactory;
            _service = service;
        }

        public void Load()
        {
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateLoanApi();
                List<LoanApplicationSummaryItem> items = _service.Get(settings).Result;
                return items;
            })
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<LoanApplicationSummaryItem>> load, object state)
        {
            try
            {
                List<LoanApplicationSummaryItem> items = await load;
                List<DateTime> xAxis = GetXAxis(items);
                SeriesCollection collection = new SeriesCollection();
                foreach (IGrouping<string, LoanApplicationSummaryItem> itemGroup in items.GroupBy(i => i.Description))
                {
                    ColumnSeries column = new ColumnSeries()
                    {
                        Title = itemGroup.Key,
                        Values = new ChartValues<int>(GetChartValues(xAxis, itemGroup))
                    };
                    collection.Add(column);
                }
                _loanApplicationSummary.XAxisLabels = xAxis.Select(a => a.ToString("yyyy MMM")).ToList();
                _loanApplicationSummary.Series = collection;
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }

        private static List<int> GetChartValues(List<DateTime> xAxis, IEnumerable<LoanApplicationSummaryItem> items)
        {
            List<int> values = new List<int>();
            foreach (DateTime month in xAxis)
            {
                int value = items.Where(i => i.Year == month.Year && i.Month == month.Month)
                    .Select(i => i.Count)
                    .Sum();
                values.Add(value);
            }
            return values;
        }

        private static List<DateTime> GetXAxis(List<LoanApplicationSummaryItem> items)
        {
            List<DateTime> axis = new List<DateTime>();
            if (items != null)
            {
                DateTime date = items
                    .Select(i => new DateTime(i.Year, i.Month, 1))
                    .OrderBy(i => i)
                    .First();
                DateTime end = items
                    .Select(i => new DateTime(i.Year, i.Month, 1))
                    .OrderByDescending(i => i)
                    .First();
                while (date <= end)
                {
                    axis.Add(date);
                    date = date.AddMonths(1);
                }
            }
            return axis;
        }
    }
}
