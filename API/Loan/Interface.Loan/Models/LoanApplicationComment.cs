using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class LoanApplicationComment
    {
        public Guid? LoanApplicationCommentId { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime? CreateTimestamp { get; set; }
    }
}
