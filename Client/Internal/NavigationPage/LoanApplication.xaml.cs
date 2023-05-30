using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
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
using Models = JestersCreditUnion.Interface.Models;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for LoanApplication.xaml
    /// </summary>
    public partial class LoanApplication : Page
    {
        private Guid? _loanApplicationId;

        public LoanApplication()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            this.Loaded += LoanApplication_Loaded;
        }

        public LoanApplication(Guid loanApplicationId)
            : this()
        {
            this.DataContext = null;
            this.LoanApplicationVM = null;
            _loanApplicationId = loanApplicationId;
        }

        LoanApplicationVM LoanApplicationVM { get; set; }

        private void LoanApplication_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loanApplicationId.HasValue)
            {
                Task.Run(() => Load(_loanApplicationId.Value))
                    .ContinueWith(LoadCallback, _loanApplicationId.Value, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private Models.LoanApplication Load(Guid loanApplicationId)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();                
                ILoanApplicationService loanApplicationService = scope.Resolve<ILoanApplicationService>();
                return loanApplicationService.Get(settingsFactory.CreateApi(), loanApplicationId).Result;
            }
        }

        private async Task LoadCallback(Task<Models.LoanApplication> load, object state)
        {
            try
            {
                Models.LoanApplication loanApplication = await load;
                this.LoanApplicationVM = LoanApplicationVM.Create(loanApplication);
                this.DataContext = this.LoanApplicationVM;
                this.LoanApplicationVM.BusyVisibility = Visibility.Collapsed;
                this.LoanApplicationVM.CommandsVisibility = Visibility.Visible;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void DenyHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                LoanApplicationDenial loanApplicationDenial = new LoanApplicationDenial(LoanApplicationDenialVM.Create(this.LoanApplicationVM.InnerLoanApplication));
                navigationService.Navigate(loanApplicationDenial);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void ApproveHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                BeginLoanAgreement beginLoanAgreement = new BeginLoanAgreement(BeginLoanAgreementVM.Create(this.LoanApplicationVM.InnerLoanApplication));
                navigationService.Navigate(beginLoanAgreement);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
