using JCU.Internal.ViewModel;
using System.Windows;

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
