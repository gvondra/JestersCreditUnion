using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class LoanPastDueVM : ViewModelBase
    {
        public ObservableCollection<LoanPastDue> Items { get; } = new ObservableCollection<LoanPastDue>();
    }
}
