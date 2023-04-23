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
