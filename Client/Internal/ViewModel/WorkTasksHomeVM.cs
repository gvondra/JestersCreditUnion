using System.Collections.ObjectModel;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class WorkTasksHomeVM : ViewModelBase
    {
        private Visibility _busyVisibility = Visibility.Collapsed;
        private readonly ObservableCollection<WorkTasksHome.WorkTaskTypeVM> _workTaskTypes = new ObservableCollection<WorkTasksHome.WorkTaskTypeVM>();

        public ObservableCollection<WorkTasksHome.WorkTaskTypeVM> WorkTaskTypes => _workTaskTypes;

        public Visibility BusyVisibility
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
