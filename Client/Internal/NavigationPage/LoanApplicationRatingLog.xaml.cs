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
    /// Interaction logic for LoanApplicationRatingLog.xaml
    /// </summary>
    public partial class LoanApplicationRatingLog : Page
    {
        public LoanApplicationRatingLog(LoanApplicationVM loanApplicationVM)
        {
            this.LoanApplicationVM = loanApplicationVM;
            this.DataContext = null;
            this.LoanApplicationRatingLogVM = null;
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            this.Loaded += LoanApplicationRatingLog_Loaded;
        }

        public LoanApplicationVM LoanApplicationVM { get; private set; }
        public LoanApplicationRatingLogVM LoanApplicationRatingLogVM { get; private set; }

        private void LoanApplicationRatingLog_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (this.LoanApplicationRatingLogVM == null)
                {
                    this.LoanApplicationRatingLogVM = new LoanApplicationRatingLogVM(this.LoanApplicationVM);
                    this.DataContext = this.LoanApplicationRatingLogVM;
                }
                if (!this.LoanApplicationRatingLogVM.ContainsBehavior<LoanApplicationRatingLogLoader>())
                {
                    LoanApplicationRatingLogLoader loader = scope.Resolve<Func<LoanApplicationRatingLogVM, LoanApplicationRatingLogLoader>>()(this.LoanApplicationRatingLogVM);
                    this.LoanApplicationRatingLogVM.AddBehavior(loader);
                    loader.Load();
                }
            }
        }
    }
}
