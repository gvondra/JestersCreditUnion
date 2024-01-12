using JCU.Internal.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for ReceiveLoanPayment.xaml
    /// </summary>
    public partial class ReceiveLoanPayment : Page
    {
        public ReceiveLoanPayment(LoanVM loan)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            ReceiveLoanPaymentVM = ReceiveLoanPaymentVM.Create(loan);
            DataContext = ReceiveLoanPaymentVM;
            InitializeComponent();
            InitializeDetailGrid();
        }

        private ReceiveLoanPaymentVM ReceiveLoanPaymentVM { get; set; }

        private void InitializeDetailGrid()
        {
            DetailGrid.RowDefinitions.Clear();
            foreach (int i in Enumerable.Range(0, 6))
            {
                DetailGrid.RowDefinitions.Add(
                    new RowDefinition() { Height = GridLength.Auto });
            }
        }
    }
}
