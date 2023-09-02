using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplicationDenial
    {
        public LoanApplicationDenialReason Reason { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }

        Task Save(ITransactionHandler transactionHandler, Guid id, LoanApplicationStatus status);
        Task<string> GetReasonDescription(ISettings settings);
    }
}
