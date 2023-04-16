using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Models;
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
    /// Interaction logic for WorkTaskFrame.xaml
    /// </summary>
    public partial class WorkTaskFrame : Page
    {
        public WorkTaskFrame()
        {
            InitializeComponent();
        }

        public WorkTaskFrame(WorkTaskVM workTaskVM)
            : this()
        {
            this.WorkTaskVM = workTaskVM;
            this.DataContext = workTaskVM;
            this.Loaded += WorkTaskFrame_Loaded;
        }

        private void WorkTaskFrame_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.WorkTaskVM != null)
            {                
                List<WorkTaskContext> contexts = this.WorkTaskVM.InnerWorkTask?.WorkTaskContexts;
                if (contexts != null && contexts.Count > 0)
                {
                    WorkTaskContext context = contexts.FirstOrDefault(c => c.ReferenceType.HasValue 
                    && !string.IsNullOrEmpty(c.ReferenceValue) 
                    && (c.ReferenceType.Value == 1));
                    switch (context.ReferenceType.Value)
                    {
                        case 1:
                            LoadLoanApplication(context);
                            break;
                    }
                }
            }
        }

        private void LoadLoanApplication(WorkTaskContext workTaskContext)
        {
            Guid loanApplicationId = Guid.Parse(workTaskContext.ReferenceValue);
            LoanApplication loanApplication = new LoanApplication(loanApplicationId);
            navigationFrame.Navigate(loanApplication);
        }

        public WorkTaskVM WorkTaskVM { get; private set; }
    }
}
