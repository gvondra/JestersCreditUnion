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
using Models = JestersCreditUnion.Interface.Models;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for LoanApplication.xaml
    /// </summary>
    public partial class LoanApplication : Page
    {
        public LoanApplication()
        {
            InitializeComponent();
        }

        public LoanApplication(Guid loanApplicationId)
            : this()
        {
            this.DataContext = null;
            this.LoanApplicationVM = null;
            Task.Run(() => Load(loanApplicationId))
                .ContinueWith(LoadCallback, loanApplicationId, TaskScheduler.FromCurrentSynchronizationContext());
        }

        LoanApplicationVM LoanApplicationVM { get; set; }

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
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
