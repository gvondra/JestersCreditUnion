using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplicationComment : ILoanApplicationComment
    {
        private readonly LoanApplicationCommentData _data;
        private readonly ILoanApplicationDataSaver _dataSaver;
        private readonly ILoanApplication _loanApplication;

        public LoanApplicationComment(LoanApplicationCommentData data, ILoanApplicationDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
            _loanApplication = null;
        }

        public LoanApplicationComment(
            LoanApplicationCommentData data,
            ILoanApplicationDataSaver dataSaver,
            ILoanApplication loanApplication)
            : this(data, dataSaver)
        {
            _loanApplication = loanApplication;
        }

        public Guid LoanApplicationCommentId => _data.LoanApplicationCommentId;

        private Guid LoanApplicationId { set => _data.LoanApplicationId = value; }

        public Guid UserId => _data.UserId;

        public bool IsInternal => _data.IsInternal;

        public string Text => _data.Text;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public Task Create(ITransactionHandler transactionHandler)
        {
            if (_loanApplication == null)
                throw new ArgumentException($"{nameof(_loanApplication)} is null");
            LoanApplicationId = _loanApplication.LoanApplicationId;
            return _dataSaver.AppendComment(transactionHandler, _data);
        }
    }
}
