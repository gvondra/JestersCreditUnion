using JCU.Internal.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for LoanAmortization.xaml
    /// </summary>
    public partial class LoanAmortization : Page
    {
        public LoanAmortization(LoanVM loan)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            LoanAmortizationVM = LoanAmortizationVM.Create(loan);
            DataContext = LoanAmortizationVM;
            InitializeComponent();
        }

        private LoanAmortizationVM LoanAmortizationVM { get; set; }
    }
}
