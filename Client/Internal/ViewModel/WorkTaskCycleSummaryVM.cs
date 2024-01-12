using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskCycleSummaryVM : ViewModelBase
    {
        public ObservableCollection<WorkTaskCycleSummaryItemVM> Items { get; } = new ObservableCollection<WorkTaskCycleSummaryItemVM>();
    }
}
