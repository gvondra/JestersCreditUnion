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
using Models = JestersCreditUnion.Interface.Loan.Models;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for PaymentIntake.xaml
    /// </summary>
    public partial class PaymentIntake : Page
    {
        public PaymentIntake()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            PaymentIntakeVM = null;
            DataContext = null;
            this.Loaded += PaymentIntake_Loaded;
        }

        public PaymentIntakeVM PaymentIntakeVM { get; private set; }

        private void PaymentIntake_Loaded(object sender, RoutedEventArgs e)
        {
            if (PaymentIntakeVM == null)
            {
                PaymentIntakeVM = new PaymentIntakeVM();
                DataContext = PaymentIntakeVM;
            }
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (PaymentIntakeVM.NewItem == null)
                {
                    PaymentIntakeVM.NewItem = new PaymentIntakeItemVM(
                        PaymentIntakeVM,
                        new Models.PaymentIntake())
                    {
                        Date = DateTime.Today,
                        Add = scope.Resolve<Behaviors.PaymentIntakeAdd>()
                    };
                    Behaviors.PaymentIntakeItemLoader loader = scope.Resolve<Func<ViewModel.PaymentIntakeItemVM, Behaviors.PaymentIntakeItemLoader>>()(PaymentIntakeVM.NewItem);
                    PaymentIntakeVM.NewItem.AddBehavior(loader);
                }
            }
        }
    }
}
