using JestersCreditUnion.CommonCore;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPaymentIntake
    {
        Guid PaymentIntakeId { get; }
        string TransactionNumber { get; set; }
        DateTime Date { get; set; }
        decimal Amount { get; set; }
        short Status { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }
        string CreateUserId { get; }
        string UpdateUserId { get; }

        Task Create(ITransactionHandler transactionHandler, string userId);
        Task Update(ITransactionHandler transactionHandler, string userId);
        Task<ILoan> GetLaon(ISettings settings);
        Task<string> GetStatusDescription(ISettings settings);
    }
}
