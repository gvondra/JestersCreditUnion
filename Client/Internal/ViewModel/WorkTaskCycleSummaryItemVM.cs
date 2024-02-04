using JestersCreditUnion.Interface.Loan.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskCycleSummaryItemVM : ViewModelBase
    {
        private const string _monthDateFormat = "yyyy-MMM";
        private readonly WorkTaskCycleSummaryItem _innerItem;

        public WorkTaskCycleSummaryItemVM(WorkTaskCycleSummaryItem innerItem)
        {
            _innerItem = innerItem;
        }

        public string CreateMonth => new DateTime(_innerItem.CreateYear, _innerItem.CreateMonth, 1).ToString(_monthDateFormat);

        public string AssignedMonth
            => _innerItem.AssignedYear.HasValue && _innerItem.AssignedMonth.HasValue
            ? new DateTime(_innerItem.AssignedYear.Value, _innerItem.AssignedMonth.Value, 1).ToString(_monthDateFormat)
            : string.Empty;

        public int? DaysToAssigment => _innerItem.DaysToAssigment;

        public string ClosedMonth
            => _innerItem.ClosedYear.HasValue && _innerItem.ClosedMonth.HasValue
            ? new DateTime(_innerItem.ClosedYear.Value, _innerItem.ClosedMonth.Value, 1).ToString(_monthDateFormat)
            : string.Empty;

        public int? DaysToClose => _innerItem.DaysToClose;

        public int? TotalDays => _innerItem.TotalDays;

        public string Title => _innerItem.Title ?? string.Empty;

        public int Count => _innerItem.Count;
    }
}
