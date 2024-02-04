using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JCU.Internal.Control
{
    /// <summary>
    /// Interaction logic for ErrorItem.xaml
    /// </summary>
    public partial class ExceptionItem : UserControl
    {
        public ExceptionItem()
        {
            InitializeComponent();
            InitializeBindings();
        }

        public string ExceptionType
        {
            get
            {
                if (DataContext == null)
                    return string.Empty;
                else
                    return DataContext.GetType().FullName;
            }
        }

        public static readonly DependencyProperty BoundDataContextProperty = DependencyProperty.Register(
            "BoundDataContextProperty",
            typeof(object),
            typeof(ExceptionItem),
            new PropertyMetadata(null, OnBoundDataContextPropertyChanged)
            );

        public static void OnBoundDataContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ExceptionItem)d).ExceptionTypeText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }

        private void InitializeBindings()
        {
            this.SetBinding(BoundDataContextProperty, new Binding());
            Binding binding = new Binding("ExceptionType");
            binding.Source = this;
            ExceptionTypeText.SetBinding(TextBlock.TextProperty, binding);
        }
    }
}
