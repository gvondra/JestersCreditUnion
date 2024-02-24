using JCU.Internal.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for BeginLoanAgreement.xaml
    /// </summary>
    public partial class BeginLoanAgreement : Page
    {
        public BeginLoanAgreement()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            InitializeDetailGrid();
        }

        public BeginLoanAgreement(BeginLoanAgreementVM beginLoanAgreementVM)
            : this()
        {            
            BeginLoanAgreementVM = beginLoanAgreementVM;
            DataContext = beginLoanAgreementVM;
            this.Loaded += BeginLoanAgreement_Loaded;
        }

        public BeginLoanAgreementVM BeginLoanAgreementVM { get; }

        private void BeginLoanAgreement_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = ParentFinder.Find(typeof(WorkTaskFrame), this);
            if (parent != null)
            {
                BeginLoanAgreementVM.NavigationService = NavigationService.GetNavigationService(parent);
            }
        }

        private void InitializeDetailGrid()
        {
            DetailGrid.RowDefinitions.Clear();
            foreach (int i in Enumerable.Range(0, 22))
            {
                DetailGrid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto });
            }
        }
    }
}
