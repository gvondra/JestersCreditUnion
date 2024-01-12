using JCU.Internal.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for LoanApplicationDenial.xaml
    /// </summary>
    public partial class LoanApplicationDenial : Page
    {
        public LoanApplicationDenial()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
        }

        public LoanApplicationDenial(LoanApplicationDenialVM loanApplicationDenialVM)
            : this()
        {
            this.LoanApplicationDenialVM = loanApplicationDenialVM;
            this.DataContext = loanApplicationDenialVM;
        }

        public LoanApplicationDenialVM LoanApplicationDenialVM { get; set; }
    }
}
