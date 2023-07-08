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

        public Loan()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            InitializeDetailGrid();
            Loaded += Loan_Loaded;
        }

        public LoanVM LoanVM { get; set; }

        public Loan(Guid loanId)
            : this()
        {
            this.DataContext = null;
            this.LoanVM = null;
            _loanId = loanId;
        }

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
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void InitializeDetailGrid()
        {
            DetailGrid.RowDefinitions.Clear();
            foreach (int i in Enumerable.Range(0, 10))
            {
                DetailGrid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto });
            }
        }
    }
}
