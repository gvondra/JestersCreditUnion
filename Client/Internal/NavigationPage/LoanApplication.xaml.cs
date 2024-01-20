using Autofac;
using JCU.Internal.Behaviors;
using JCU.Internal.Constants;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Models = JestersCreditUnion.Interface.Loan.Models;

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
            this.DataContext = null;
            this.LoanApplicationVM = null;
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            InitializeDetailGrid();
            this.Loaded += LoanApplication_Loaded;
        }

        public LoanApplication(Guid loanApplicationId)
            : this()
        {
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
                return loanApplicationService.Get(settingsFactory.CreateLoanApi(), loanApplicationId).Result;
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

                IdentificationCardLoader identificationCardLoader = new IdentificationCardLoader(this.LoanApplicationVM);
                this.LoanApplicationVM.AddBehavior(identificationCardLoader);
                identificationCardLoader.LoadBorrowerIdentificationCard();

                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    LoanApplicationLoader loanApplicationLoader = scope.Resolve<Func<LoanApplicationVM, LoanApplicationLoader>>()(this.LoanApplicationVM);
                    this.LoanApplicationVM.AddBehavior(loanApplicationLoader);
                    loanApplicationLoader.LoadRating();
                }
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
                if (this.LoanApplicationVM.Status != LoanApplicationStatuses.Approved)
                {
                    Task.Run(() => ApproveLoanApplication(this.LoanApplicationVM))
                        .ContinueWith(ApproveLoanApplicationCallback, this.LoanApplicationVM, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    Task.Run(() => GetLoan(this.LoanApplicationVM.LoanApplicationId.Value))
                        .ContinueWith(GetLoanCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void ApproveLoanApplication(LoanApplicationVM loanApplicationVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ILoanApplicationService loanApplicationService = scope.Resolve<ILoanApplicationService>();
                loanApplicationVM.Status = LoanApplicationStatuses.Approved;
                loanApplicationService.Update(settingsFactory.CreateLoanApi(), loanApplicationVM.InnerLoanApplication).Wait();
            }
        }

        private async Task ApproveLoanApplicationCallback(Task approveLoanApplication, object state)
        {
            try
            {
                await approveLoanApplication;
                LoanApplicationVM loanApplicationVM = (LoanApplicationVM)state;
                _ = Task.Run(() => GetLoan(loanApplicationVM.LoanApplicationId.Value))
                    .ContinueWith(GetLoanCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private Models.Loan GetLoan(Guid loanApplicationId)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ILoanService loanService = scope.Resolve<ILoanService>();
                return loanService.GetByLoanApplicationId(settingsFactory.CreateLoanApi(), loanApplicationId).Result;
            }
        }

        private async Task GetLoanCallback(Task<Models.Loan> getLoan, object state)
        {
            try
            {
                Models.Loan loan = await getLoan;

                BeginLoanAgreement beginLoanAgreement;
                if (loan is null)
                {
                    beginLoanAgreement = new BeginLoanAgreement(BeginLoanAgreementVM.Create(this.LoanApplicationVM.InnerLoanApplication));
                }                
                else
                {
                    beginLoanAgreement = new BeginLoanAgreement(BeginLoanAgreementVM.Create(loan));
                }
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(beginLoanAgreement);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
        private void InitializeDetailGrid()
        {
            DetailGrid.RowDefinitions.Clear();
            foreach (int i in Enumerable.Range(0, 39))
            {
                DetailGrid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto });
            }
        }
    }
}
