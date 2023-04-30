using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
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

        public Guid UserId => _data.UserId;

        public bool IsInternal => _data.IsInternal;

        public string Text => _data.Text;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public Task Create(ISettings settings)
        {
            if (_loanApplication == null)
                throw new ArgumentNullException(nameof(_loanApplication));
            return _dataSaver.AppendComment(new DataSettings(settings), _loanApplication.LoanApplicationId, _data);
        }
    }
}
