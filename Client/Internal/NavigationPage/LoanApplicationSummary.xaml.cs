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
    /// Interaction logic for LoanApplicationSummary.xaml
    /// </summary>
    public partial class LoanApplicationSummary : Page
    {
        public LoanApplicationSummary()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            LoanApplicationSummaryVM = null;
            this.Loaded += LoanApplicationSummary_Loaded;
        }

        internal LoanApplicationSummaryVM LoanApplicationSummaryVM { get; private set; }

        private void LoanApplicationSummary_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                LoanApplicationSummaryVM = new LoanApplicationSummaryVM();
                DataContext = LoanApplicationSummaryVM;
                LoanApplicationSummaryLoader loader = scope.Resolve<Func<LoanApplicationSummaryVM, LoanApplicationSummaryLoader>>()(LoanApplicationSummaryVM);
                LoanApplicationSummaryVM.AddBehavior(loader);
                loader.Load();
            }
        }
    }
}
