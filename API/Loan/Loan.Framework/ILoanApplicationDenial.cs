using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanApplicationDenial
    {
        public LoanApplicationDenialReason Reason { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }

        Task Save(ITransactionHandler transactionHandler, Guid id, LoanApplicationStatus status, DateTime? closedDate);
        Task<string> GetReasonDescription(ISettings settings);
    }
}
