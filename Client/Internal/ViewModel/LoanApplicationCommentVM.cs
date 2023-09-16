using JestersCreditUnion.Interface.Loan.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationCommentVM : ViewModelBase
    {
        private readonly LoanApplicationComment _innerComment;
        private readonly LoanApplicationVM _loanApplication;

        private LoanApplicationCommentVM(LoanApplicationComment comment, LoanApplicationVM loanApplicationVM)
        {
            _innerComment = comment;
            _loanApplication = loanApplicationVM;
        }

        public int RowIndex => _loanApplication.Comments.IndexOf(this);

        public string UserName
        {
            get => _innerComment.UserName;
            set
            {
                if (_innerComment.UserName != value)
                {
                    _innerComment.UserName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Text 
        { 
            get => _innerComment.Text;
            set
            {
                if (_innerComment.Text != value)
                {
                    _innerComment.Text = value;
                    NotifyPropertyChanged();
                }
            } 
        }
        public DateTime CreateTimestamp 
        { 
            get => _innerComment.CreateTimestamp ?? DateTime.UtcNow;
            set
            {
                if (!_innerComment.CreateTimestamp.HasValue || _innerComment.CreateTimestamp.Value != value)
                {
                    _innerComment.CreateTimestamp = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        public static LoanApplicationCommentVM Create(LoanApplicationComment comment, LoanApplicationVM loanApplicationVM)
        {
            LoanApplicationCommentVM vm = new LoanApplicationCommentVM(comment, loanApplicationVM);
            return vm;
        }
    }
}
