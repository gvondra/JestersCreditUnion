using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for WorkTaskType.xaml
    /// </summary>
    public partial class WorkTaskType : Page
    {
        public WorkTaskType(WorkTaskTypeVM workTaskTypeVM)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            this.WorkTaskTypeVM = workTaskTypeVM;
            this.DataContext = workTaskTypeVM;
        }

        private WorkTaskTypeVM WorkTaskTypeVM { get; set; }

        private void AddStatusButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    WorkTaskStatusVM workTaskStatusVM = WorkTaskStatusVM.Create(this.WorkTaskTypeVM, 
                        scope.Resolve<ISettingsFactory>(),
                        scope.Resolve<IWorkTaskStatusService>());
                    this.WorkTaskTypeVM.WorkTaskStatusesVM.Items.Add(workTaskStatusVM);
                    this.WorkTaskTypeVM.WorkTaskStatusesVM.SelectedItem = workTaskStatusVM;
                }                
            }
            catch(System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
