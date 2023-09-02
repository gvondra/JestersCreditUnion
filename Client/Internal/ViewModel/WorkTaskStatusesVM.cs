using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskStatusesVM : ViewModelBase
    {
        private readonly WorkTaskTypeVM _workTaskTypeVM;
        private readonly ObservableCollection<WorkTaskStatusVM> _items = new ObservableCollection<WorkTaskStatusVM>();
        private WorkTaskStatusesLoader _workTaskStatusesLoader;
        private WorkTaskStatusVM _selectedItem;

        private WorkTaskStatusesVM(WorkTaskTypeVM workTaskTypeVM)
        {
            _workTaskTypeVM = workTaskTypeVM;
        }

        public WorkTaskTypeVM WorkTaskTypeVM => _workTaskTypeVM;

        public ObservableCollection<WorkTaskStatusVM> Items => _items;

        public WorkTaskStatusVM SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public WorkTaskStatusesLoader WorkTaskStatusesLoader
        {
            get => _workTaskStatusesLoader;
            set
            {
                _workTaskStatusesLoader = value;
                NotifyPropertyChanged();
            }
        }

        public static WorkTaskStatusesVM Create(WorkTaskTypeVM workTaskTypeVM, ISettingsFactory settingsFactory, IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskStatusesVM vm = new WorkTaskStatusesVM(workTaskTypeVM);
            vm.WorkTaskStatusesLoader = new WorkTaskStatusesLoader(settingsFactory, workTaskStatusService);
            return vm;
        }
    }
}
