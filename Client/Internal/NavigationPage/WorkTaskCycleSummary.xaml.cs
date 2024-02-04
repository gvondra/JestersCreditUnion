using Autofac;
using JCU.Internal.Behaviors;
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
    /// Interaction logic for WorkTaskCycleSummary.xaml
    /// </summary>
    public partial class WorkTaskCycleSummary : Page
    {
        public WorkTaskCycleSummary()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            WorkTaskCycleSummaryVM = null;
            this.Loaded += WorkTaskCycleSummary_Loaded;
        }

        public WorkTaskCycleSummaryVM WorkTaskCycleSummaryVM { get; private set; }

        private void WorkTaskCycleSummary_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (WorkTaskCycleSummaryVM == null)
                {
                    WorkTaskCycleSummaryVM = new WorkTaskCycleSummaryVM();
                    DataContext = WorkTaskCycleSummaryVM;
                }
                if (!WorkTaskCycleSummaryVM.ContainsBehavior<WorkTaskCycleSummaryLoader>())
                {
                    WorkTaskCycleSummaryLoader loader = scope.Resolve<Func<WorkTaskCycleSummaryVM, WorkTaskCycleSummaryLoader>>()(WorkTaskCycleSummaryVM);
                    WorkTaskCycleSummaryVM.AddBehavior(loader);
                    loader.Load();
                }
            }
        }
    }
}
