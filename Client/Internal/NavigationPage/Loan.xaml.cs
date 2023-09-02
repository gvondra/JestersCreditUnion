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
    /// Interaction logic for Loan.xaml
    /// </summary>
    public partial class Loan : Page
    {
        private Guid? _loanId;
        private Visibility _backVisibility = Visibility.Collapsed;

        public Loan()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            this.DataContext = null;
            this.LoanVM = null;
            InitializeComponent();
            InitializeDetailGrid();
            Loaded += Loan_Loaded;
        }

        public Loan(
            Guid loanId,
            Visibility backVisibility = Visibility.Collapsed)
            : this()
        {
            _loanId = loanId;
            _backVisibility = backVisibility;
        }

        public LoanVM LoanVM { get; set; }

        private void Loan_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loanId.HasValue)
            {
                Task.Run(() => Load(_loanId.Value))
                    .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private static LoanVM Load(Guid loanId)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateApi();
                ILoanService loanService = scope.Resolve<ILoanService>();
                Models.Loan loan = loanService.Get(settings, loanId).Result;
                return LoanVM.Create(loan);
            }
        }

        private async Task LoadCallback(Task<LoanVM> load, object state)
        {
            try
            {
                this.LoanVM = await load;
                this.DataContext = this.LoanVM;
                this.LoanVM.BackVisibility = _backVisibility;
                this.LoanVM.CommandsVisibility = Visibility.Visible;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
            finally
            {
                this.LoanVM.BusyVisibility = Visibility.Collapsed;
            }
        }

        private void InitializeDetailGrid()
        {
            DetailGrid.RowDefinitions.Clear();
            foreach (int i in Enumerable.Range(0, 12))
            {
                DetailGrid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto });
            }
        }

        private void ReceivePaymentHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReceiveLoanPayment receiveLoanPayment = new ReceiveLoanPayment(LoanVM);
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(receiveLoanPayment);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void AmortizationHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoanAmortization loanAmortization = new LoanAmortization(LoanVM);
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(loanAmortization);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
