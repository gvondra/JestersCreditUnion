using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage.Log
{
    /// <summary>
    /// Interaction logic for MetricLog.xaml
    /// </summary>
    public partial class MetricLog : Page
    {
        public MetricLog()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += MetricLog_Loaded;                     
        }

        private void MetricLog_Loaded(object sender, RoutedEventArgs e)
        {
            GoogleLogin.ShowLoginDialog(owner: Window.GetWindow(this));
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                MetricLogVM = MetricLogVM.Create(scope.Resolve<ISettingsFactory>(), scope.Resolve<IMetricService>());
                DataContext = MetricLogVM;
            }
        }

        private MetricLogVM MetricLogVM { get; set; }
    }
}
