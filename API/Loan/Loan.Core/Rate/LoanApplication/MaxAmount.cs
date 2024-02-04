using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace JestersCreditUnion.Loan.Core.Rate.LoanApplication
{
    public class MaxAmount : IComponent
    {
        private readonly IRatingFactory _ratingFactory;

        public MaxAmount(IRatingFactory ratingFactory, int points)
        {
            _ratingFactory = ratingFactory;
            Points = points;
        }

        public int Points { get; set; }
        internal decimal Maximum { get; set; }
        internal string LogMessageTemplage { get; set; }

        public IEnumerable<IRatingLog> Evaluate(ILoanApplication loanApplication, int totalPoints)
        {
            (string, double) result = Maximum < loanApplication.Amount
                ? (RatingLogStatus.Fail, 0.0)
                : (RatingLogStatus.Pass, Math.Round(Points / (double)totalPoints, 3));
            return [_ratingFactory.CreateLog(result.Item2, string.Format(CultureInfo.InvariantCulture, LogMessageTemplage, result.Item1, loanApplication.Amount, Maximum))];
        }
    }
}
