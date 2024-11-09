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

namespace JCU.Internal.Control
{
    /// <summary>
    /// Interaction logic for PaymentIntake.xaml
    /// </summary>
    public partial class PaymentIntake : UserControl
    {
        public PaymentIntake()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoanNumberTextBox.Focus();
        }
    }
}
