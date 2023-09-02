using Autofac;
using JCU.Internal.NavigationPage;
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

namespace JCU.Internal.Control
{
    /// <summary>
    /// Interaction logic for WorkTasksHome.xaml
    /// </summary>
    public partial class WorkTasksHome : UserControl
    {
        public WorkTasksHome()
        {
            InitializeComponent();
            this.Loaded += WorkTasksHome_Loaded;
        }

        private void WorkTasksHome_Loaded(object sender, RoutedEventArgs e)
        {
            GoogleLogin.ShowLoginDialog(true, Window.GetWindow(this));
        }

        private void OpenHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    JCU.Internal.ViewModel.WorkTasksHome.WorkTaskVM vm = (JCU.Internal.ViewModel.WorkTasksHome.WorkTaskVM)((Hyperlink)sender).DataContext;
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    IWorkTaskTypeService workTaskTypeService = scope.Resolve<IWorkTaskTypeService>();
                    IWorkTaskStatusService workTaskStatusService = scope.Resolve<IWorkTaskStatusService>();
                    WorkTaskVM workTaskVM = WorkTaskVM.Create(vm.InnerWorkTask, settingsFactory, workTaskTypeService, workTaskStatusService);
                    if (workTaskVM.WorkTaskTypeMV.WorkTaskStatusesVM.WorkTaskStatusesLoader != null)
                    {
                        workTaskVM.WorkTaskTypeMV.WorkTaskStatusesVM.WorkTaskStatusesLoader.SelectedStatusId = workTaskVM.WorkTaskStatusVM?.WorkTaskStatusId;
                        workTaskVM.WorkTaskTypeMV.WorkTaskStatusesVM.WorkTaskStatusesLoader.Execute(workTaskVM.WorkTaskTypeMV.WorkTaskStatusesVM);
                    }                        
                    NavigationService navigationService = NavigationService.GetNavigationService(this.Parent);
                    WorkTaskFrame workTask = new WorkTaskFrame(workTaskVM);
                    navigationService.Navigate(workTask);
                }                    
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
