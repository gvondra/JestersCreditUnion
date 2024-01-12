using JCU.Internal.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for Lookups.xaml
    /// </summary>
    public partial class Lookups : Page
    {
        public Lookups()
        {
            this.DataContext = null;
            InitializeComponent();
            this.Loaded += Lookups_Loaded;
        }

        public LookupsVM LookupsVM { get; private set; }

        private void Lookups_Loaded(object sender, RoutedEventArgs e)
        {
            this.LookupsVM = LookupsVM.Create();
            this.DataContext = this.LookupsVM;
        }
    }
}
