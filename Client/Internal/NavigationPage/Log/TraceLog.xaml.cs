using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage.Log
{
    /// <summary>
    /// Interaction logic for TraceLog.xaml
    /// </summary>
    public partial class TraceLog : Page
    {
        public TraceLog()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += TraceLog_Loaded;
        }

        private TraceLogVM TraceLogVM { get; set; }

        private void TraceLog_Loaded(object sender, RoutedEventArgs e)
        {
            using(ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ITraceService traceService = scope.Resolve<ITraceService>();
                this.TraceLogVM = TraceLogVM.Create(settingsFactory, traceService);
                DataContext = this.TraceLogVM;
            }
        }
    }
}
