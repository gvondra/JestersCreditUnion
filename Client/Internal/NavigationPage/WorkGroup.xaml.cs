using JCU.Internal.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for WorkGroup.xaml
    /// </summary>
    public partial class WorkGroup : Page
    {
        public WorkGroup(WorkGroupVM workGroupVM)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            this.WorkGroupVM = workGroupVM;
            this.DataContext = workGroupVM;
        }

        private WorkGroupVM WorkGroupVM { get; set; }
    }
}
