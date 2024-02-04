using Autofac;
using JCU.Internal.Behaviors;
using JCU.Internal.ViewModel;
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
    /// Interaction logic for OpenLoanSummary.xaml
    /// </summary>
    public partial class OpenLoanSummary : Page
    {
        public OpenLoanSummary()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            OpenLoanSummaryVM = null;
            this.Loaded += OpenLoanSummary_Loaded;
        }

        public OpenLoanSummaryVM OpenLoanSummaryVM { get; private set; }

        private void OpenLoanSummary_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (OpenLoanSummaryVM == null)
                {
                    OpenLoanSummaryVM = new OpenLoanSummaryVM();
                    DataContext = OpenLoanSummaryVM;
                }
                if (!OpenLoanSummaryVM.ContainsBehavior<OpenLoanSummaryLoader>())
                {
                    OpenLoanSummaryLoader loader = scope.Resolve<Func<OpenLoanSummaryVM, OpenLoanSummaryLoader>>()(OpenLoanSummaryVM);
                    OpenLoanSummaryVM.AddBehavior(loader);
                    loader.Load();
                }
            }
        }
    }
}
