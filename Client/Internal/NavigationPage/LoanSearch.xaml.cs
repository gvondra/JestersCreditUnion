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
    /// Interaction logic for LoanSearch.xaml
    /// </summary>
    public partial class LoanSearch : Page
    {
        public LoanSearch()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            LoanSearchVM = null;
            Loaded += LoanSearch_Loaded;
        }

        public LoanSearchVM LoanSearchVM { get; set; }

        private void LoanSearch_Loaded(object sender, RoutedEventArgs e)
        {
            LoanSearchVM = LoanSearchVM.Create();
            DataContext = LoanSearchVM;
        }
    }
}
