using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for WorkGroup.xaml
    /// </summary>
    public partial class WorkGroups : Page
    {
        public WorkGroups()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += WorkGroups_Loaded;
        }

        private WorkGroupsVM WorkGroupsVM { get; set; }

        private void WorkGroups_Loaded(object sender, RoutedEventArgs e)
        {
            if (WorkGroupsVM == null)
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    IWorkGroupService workGroupService = scope.Resolve<IWorkGroupService>();
                    IUserService userService = scope.Resolve<IUserService>();
                    IWorkTaskTypeService workTaskTypeService = scope.Resolve<IWorkTaskTypeService>();
                    this.WorkGroupsVM = WorkGroupsVM.Create(settingsFactory, workGroupService, userService, workTaskTypeService);
                    this.DataContext = this.WorkGroupsVM;
                }
            }
        }

        private void CreateHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    IWorkGroupService workGroupService = scope.Resolve<IWorkGroupService>();
                    IUserService userService = scope.Resolve<IUserService>();
                    IWorkTaskTypeService workTaskTypeService = scope.Resolve<IWorkTaskTypeService>();
                    NavigationService navigationService = NavigationService.GetNavigationService(this);
                    WorkGroup workGroup = new WorkGroup(WorkGroupVM.Create(settingsFactory, workGroupService, userService, workTaskTypeService));
                    navigationService.Navigate(workGroup);
                    this.WorkGroupsVM = null;
                    this.DataContext = null;
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender;
                if (dataGrid.SelectedItem != null)
                {
                    NavigationService navigationService = NavigationService.GetNavigationService(this);
                    WorkGroupVM workGroupVM = (WorkGroupVM)dataGrid.SelectedItem;
                    if (workGroupVM.LoadMembers != null)
                        workGroupVM.LoadMembers();
                    if (workGroupVM.LoadTypes != null)
                        workGroupVM.LoadTypes();
                    WorkGroup workGroup = new WorkGroup(workGroupVM);
                    navigationService.Navigate(workGroup);
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
