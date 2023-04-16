using JCU.Internal.NavigationPage;
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

namespace JCU.Internal.Control
{
    /// <summary>
    /// Interaction logic for WorkTasksHome.xaml
    /// </summary>
    public partial class WorkTasksHome : UserControl
    {
        public WorkTasksHome()
        {
            InitializeComponent();
        }

        private void OpenHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JCU.Internal.ViewModel.WorkTasksHome.WorkTaskVM vm = (JCU.Internal.ViewModel.WorkTasksHome.WorkTaskVM)((Hyperlink)sender).DataContext;
                WorkTaskVM workTaskVM = WorkTaskVM.Create(vm.InnerWorkTask);
                NavigationService navigationService = NavigationService.GetNavigationService(this.Parent);
                WorkTaskFrame workTask = new WorkTaskFrame(workTaskVM);
                navigationService.Navigate(workTask);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
