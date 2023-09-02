using Autofac;
using JCU.Internal.Behaviors;
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
    /// Interaction logic for WorkTaskConfiguration.xaml
    /// </summary>
    public partial class WorkTaskConfiguration : Page
    {
        private WorkTaskConfigurationLoader _loader;

        public WorkTaskConfiguration()
        {            
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += WorkTaskConfiguration_Loaded;
        }

        private WorkTaskConfigurationVM WorkTaskConfigurationVM { get; set; }   

        private void WorkTaskConfiguration_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                IWorkTaskConfigurationService configService = scope.Resolve<IWorkTaskConfigurationService>();
                _loader = new WorkTaskConfigurationLoader(settingsFactory, configService);
                _loader.Load(LoadCallback);
            }
        }

        private void LoadCallback(WorkTaskConfigurationVM workTaskConfigurationVM)
        {
            this.WorkTaskConfigurationVM = workTaskConfigurationVM;
            DataContext = this.WorkTaskConfigurationVM;
        }
    }
}
