using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class RatingSaver : IRatingSaver
    {
        public Task SaveLoanApplicationRating(ISettings settings, Guid loanApplicationId, IRating rating)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), th => rating.SaveLoanApplication(th, loanApplicationId));
    }
}
