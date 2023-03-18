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

namespace JCU.Internal.NavigationPage.Log
{
    /// <summary>
    /// Interaction logic for ErrorLog.xaml
    /// </summary>
    public partial class ErrorLog : Page
    {
        public ErrorLog()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += ErrorLog_Loaded;
        }

        private ErrorLogVM ErrorLogVM { get; set; }

        private void ErrorLog_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                IExceptionService exceptionService = scope.Resolve<IExceptionService>();
                ErrorLogVM = ErrorLogVM.Create(settingsFactory, exceptionService);
                DataContext = ErrorLogVM;
            }
        }
    }
}
