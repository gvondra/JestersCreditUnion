using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
                    && !string.IsNullOrEmpty(c.ReferenceValue));
                    if (context != null)
                    {
                        switch (context.ReferenceType.Value)
                        {
                            case Constants.WorkTaskContextTypes.LoanApplicationId:
                                LoadLoanApplication(context);
                                break;
                            case Constants.WorkTaskContextTypes.LoanId:
                                LoadLoan(context);
                                break;
                        }
                    }
                }
            }
        }

        private void LoadLoan(WorkTaskContext workTaskContext)
        {
            Guid loanId = Guid.Parse(workTaskContext.ReferenceValue);
            Loan loan = new Loan(loanId);
            navigationFrame.Navigate(loan);
        }

        private void LoadLoanApplication(WorkTaskContext workTaskContext)
        {
            Guid loanApplicationId = Guid.Parse(workTaskContext.ReferenceValue);
            LoanApplication loanApplication = new LoanApplication(loanApplicationId);
            navigationFrame.Navigate(loanApplication);
        }

        public WorkTaskVM WorkTaskVM { get; private set; }

        private void ReleaseHyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (this.WorkTaskVM?.Release != null)
            {
                this.WorkTaskVM.Release.Execute(this.WorkTaskVM);
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(new Uri("NavigationPage/Home.xaml", UriKind.Relative));
            }
        }
    }
}
