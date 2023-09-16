using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class CreateLoanApplicationComment : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(LoanApplicationVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Parameter must be of type {typeof(LoanApplicationVM).Name}");
            LoanApplicationVM loanApplicationVM = (LoanApplicationVM)parameter;
            if (string.IsNullOrEmpty(loanApplicationVM.NewCommentText))
            {
                loanApplicationVM[nameof(LoanApplicationVM.NewCommentText)] = "Comment text not set";
            }
            else
            {
                _canExecute = false;
                this.CanExecuteChanged.Invoke(this, new EventArgs());
                loanApplicationVM[nameof(LoanApplicationVM.NewCommentText)] = null;
                Task.Run(() => CreateComment(loanApplicationVM.LoanApplicationId.Value, loanApplicationVM.NewCommentText, loanApplicationVM.NewCommentIsPublic))
                    .ContinueWith(CreateCommentCallback, loanApplicationVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private LoanApplicationComment CreateComment(Guid id, string text, bool isPublic)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ILoanApplicationService loanApplicationService = scope.Resolve<ILoanApplicationService>();
                LoanApplicationComment comment = new LoanApplicationComment
                {
                    Text = text
                };
                return loanApplicationService.AppendComent(
                    settingsFactory.CreateLoanApi(),
                    id,
                    comment,
                    isPublic).Result;
            }
        }

        private async Task CreateCommentCallback(Task<LoanApplicationComment> createComment, object state)
        {            
            try
            {
                if (state == null)
                    throw new ArgumentNullException(nameof(state));
                LoanApplicationVM loanApplicationVM = (LoanApplicationVM)state;
                LoanApplicationComment comment = await createComment;
                loanApplicationVM.Comments.Insert(0, LoanApplicationCommentVM.Create(comment, loanApplicationVM));
                loanApplicationVM.NewCommentText = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                this.CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }
    }
}
