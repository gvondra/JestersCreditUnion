using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Windows;
using System.Windows.Documents;

namespace JCU.Internal.ViewModel
{
    public class HomeVM : ViewModelBase
    {
        private WorkTasksHomeVM _workTasksHomeVM = new WorkTasksHomeVM();
        private Visibility _workTasksHomeVisibility = Visibility.Collapsed;
        private Visibility _searchLoansLinkVisibility = Visibility.Collapsed;
        private FlowDocument _document;

        private HomeVM() { }

        public Action LoadWorkTasks { get; set; }

        public FlowDocument Document
        {
            get => _document;
            set
            {
                if (_document != value)
                {
                    _document = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public WorkTasksHomeVM WorkTasksHomeVM
        {
            get => _workTasksHomeVM;
            set
            {
                if (_workTasksHomeVM != value)
                {
                    _workTasksHomeVM = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility WorkTasksHomeVisibility
        {
            get => _workTasksHomeVisibility;
            set
            {
                if (_workTasksHomeVisibility != value)
                {
                    _workTasksHomeVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility SearchLoansLinkVisibility
        {
            get => _searchLoansLinkVisibility;
            set
            {
                if (_searchLoansLinkVisibility != value)
                {
                    _searchLoansLinkVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static HomeVM Create(ISettingsFactory settingsFactory,
            IWorkGroupService workGroupService,
            IUserService userService,
            IWorkTaskService workTaskService)
        {
            HomeVM vm = new HomeVM();
            WorkTasksHomeLoader loader = new WorkTasksHomeLoader(vm.WorkTasksHomeVM, settingsFactory, workGroupService, userService, workTaskService);
            vm.AddBehavior(loader);
            vm.LoadWorkTasks = loader.Load;
            return vm;
        }
    }
}
