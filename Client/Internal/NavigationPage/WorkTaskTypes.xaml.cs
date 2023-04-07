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
    /// Interaction logic for WorkTaskTypes.xaml
    /// </summary>
    public partial class WorkTaskTypes : Page
    {
        public WorkTaskTypes()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += WorkTaskTypes_Loaded;
        }

        private WorkTaskTypesVM WorkTaskTypesVM { get; set; }

        private void WorkTaskTypes_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.WorkTaskTypesVM == null)
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    this.WorkTaskTypesVM = WorkTaskTypesVM.Create(
                        scope.Resolve<ISettingsFactory>(),
                        scope.Resolve<IWorkTaskTypeService>(),
                        scope.Resolve<IWorkTaskStatusService>()
                        );
                    DataContext = this.WorkTaskTypesVM;
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
                    IWorkTaskTypeService workTaskTypeService = scope.Resolve<IWorkTaskTypeService>();
                    IWorkTaskStatusService workTaskStatusService = scope.Resolve<IWorkTaskStatusService>();
                    NavigationService navigationService = NavigationService.GetNavigationService(this);
                    WorkTaskType workTaskType = new WorkTaskType(WorkTaskTypeVM.Create(settingsFactory, workTaskTypeService, workTaskStatusService));
                    navigationService.Navigate(workTaskType);
                    this.WorkTaskTypesVM = null;
                    this.DataContext = null;
                }
            }
            catch(Exception ex)
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
                    using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                    {
                        ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                        IWorkTaskTypeService workTaskTypeService = scope.Resolve<IWorkTaskTypeService>();
                        NavigationService navigationService = NavigationService.GetNavigationService(this);
                        WorkTaskTypeVM workTaskTypeVM = (WorkTaskTypeVM)dataGrid.SelectedItem;
                        WorkTaskType workTaskType = new WorkTaskType(workTaskTypeVM);
                        navigationService.Navigate(workTaskType);
                        workTaskTypeVM.WorkTaskStatusesVM.WorkTaskStatusesLoader.Execute(workTaskTypeVM.WorkTaskStatusesVM);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
