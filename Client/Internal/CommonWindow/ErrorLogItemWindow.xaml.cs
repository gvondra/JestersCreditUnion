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
using System.Windows.Shapes;

namespace JCU.Internal.CommonWindow
{
    /// <summary>
    /// Interaction logic for ErrorLogItemWindow.xaml
    /// </summary>
    public partial class ErrorLogItemWindow : Window
    {
        private ExceptionLogItemWindowVM _exceptionLogItemWindowVM;
        public ErrorLogItemWindow()
        {
            InitializeComponent();
        }

        public ExceptionLogItemWindowVM ExceptionLogItemWindowVM
        {
            get => _exceptionLogItemWindowVM;
            set
            {
                _exceptionLogItemWindowVM = value;
                DataContext = value;
            }
        }
    }
}
