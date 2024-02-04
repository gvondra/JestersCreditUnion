using JestersCreditUnion.Loan.Core.Rate.LoanApplication;
using JestersCreditUnion.Loan.Framework;
using System.Collections.Generic;
using System.Linq;
using RateLoanApplication = JestersCreditUnion.Loan.Core.Rate.LoanApplication;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplicationRater : ILoanApplicationRater
    {
        private readonly IRatingFactory _ratingFactory;
        private readonly RateLoanApplication.IComponent _raterComponent;

        internal LoanApplicationRater(
            IRatingFactory ratingFactory,
            IComponent raterComponent)
        {
            _ratingFactory = ratingFactory;
            _raterComponent = raterComponent;
        }

        public IRating Rate(ILoanApplication loanApplication)
        {
            IEnumerable<IRatingLog> ratingLogs = _raterComponent.Evaluate(loanApplication, _raterComponent.Points);
            return _ratingFactory.Create(ratingLogs.Sum(rl => rl.Value ?? 0.0), ratingLogs);
        }
    }
}
