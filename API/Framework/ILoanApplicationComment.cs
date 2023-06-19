using JestersCreditUnion.CommonCore;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplicationComment
    {
        public Guid LoanApplicationCommentId { get; }
        public Guid UserId { get; }
        public bool IsInternal { get; }
        public string Text { get; }
        public DateTime CreateTimestamp { get; }

        Task Create(ITransactionHandler transactionHandler);
    }
}
