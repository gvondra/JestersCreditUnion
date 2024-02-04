using JestersCreditUnion.CommonCore;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IRating
    {
        Guid RatingId { get; }
        double Value { get; set; }
        DateTime CreateTimestamp { get; }
        ImmutableList<IRatingLog> RatingLogs { get; }

        Task SaveLoanApplication(ITransactionHandler transactionHandler, Guid loanApplicationId);
    }
}
