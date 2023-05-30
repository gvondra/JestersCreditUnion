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
    /// Interaction logic for BeginLoanAgreement.xaml
    /// </summary>
    public partial class BeginLoanAgreement : Page
    {
        public BeginLoanAgreement()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
        }

        public BeginLoanAgreement(BeginLoanAgreementVM beginLoanAgreementVM)
            : this()
        {
            BeginLoanAgreementVM = beginLoanAgreementVM;
            DataContext = beginLoanAgreementVM;
        }

        public BeginLoanAgreementVM BeginLoanAgreementVM { get; set; }
    }
}
