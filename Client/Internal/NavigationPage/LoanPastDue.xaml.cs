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
    /// Interaction logic for LoanPastDue.xaml
    /// </summary>
    public partial class LoanPastDue : Page
    {
        public LoanPastDue()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            LoanPastDueVM = null;
            DataContext = null;
            this.Loaded += LoanPastDue_Loaded;
        }

        public LoanPastDueVM LoanPastDueVM { get; private set; }

        private void LoanPastDue_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoanPastDueVM == null)
            {
                LoanPastDueVM = new LoanPastDueVM();
                DataContext = LoanPastDueVM;
            }
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (!LoanPastDueVM.ContainsBehavior<LoanPastDueLoader>())
                {
                    LoanPastDueLoader loader = scope.Resolve<Func<LoanPastDueVM, LoanPastDueLoader>>()(LoanPastDueVM);
                    LoanPastDueVM.AddBehavior(loader);
                    loader.Load();
                }
            }
        }
    }
}
