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
    /// Interaction logic for InterestRateConfiguration.xaml
    /// </summary>
    public partial class InterestRateConfiguration : Page
    {
        public InterestRateConfiguration()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            InterestRateConfigurationVM = null;
            this.Loaded += InterestRateConfiguration_Loaded;
        }

        public InterestRateConfigurationVM InterestRateConfigurationVM { get; private set; }

        private void InterestRateConfiguration_Loaded(object sender, RoutedEventArgs e)
        {
            if (InterestRateConfigurationVM == null)
            {
                InterestRateConfigurationVM = new InterestRateConfigurationVM();
                DataContext = InterestRateConfigurationVM;
            }
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (!InterestRateConfigurationVM.ContainsBehavior<InterestRateConfigurationValidator>())
                {
                    InterestRateConfigurationValidator validator = scope.Resolve<Func<InterestRateConfigurationVM, InterestRateConfigurationValidator>>()(InterestRateConfigurationVM);
                    InterestRateConfigurationVM.AddBehavior(validator);
                }
                if (InterestRateConfigurationVM.Load == null)
                {
                    InterestRateConfigurationVM.Load = scope.Resolve<InterestRateConfigurationLoader>();
                    InterestRateConfigurationVM.Load.Execute(InterestRateConfigurationVM);
                }
                if (InterestRateConfigurationVM.Save == null)
                {
                    InterestRateConfigurationVM.Save = scope.Resolve<InterestRateConfigurationSaver>();
                }
            }
        }
    }
}
