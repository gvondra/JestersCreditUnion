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
