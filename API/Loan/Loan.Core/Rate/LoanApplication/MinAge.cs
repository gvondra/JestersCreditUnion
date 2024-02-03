using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace JestersCreditUnion.Loan.Core.Rate.LoanApplication
{
    internal sealed class MinAge : IComponent
    {
        private readonly IRatingFactory _ratingFactory;

        public MinAge(IRatingFactory ratingFactory, int points)
        {
            _ratingFactory = ratingFactory;
            Points = points;
        }

        public int Points { get; set; }
        internal int Minimum { get; set; }
        internal string LogMessageTemplage { get; set; }
        internal bool BorrowerAge { get; set; }
        internal bool CoBorrowerAge { get; set; }

        public IEnumerable<IRatingLog> Evaluate(ILoanApplication loanApplication, int totalPoints)
        {
            int? age;
            if (BorrowerAge)
                age = loanApplication.GetBorrowerAge();
            else if (CoBorrowerAge)
                age = loanApplication.GetCoBorrowerAge();
            else
                age = loanApplication.GetBorrowerAge();

            (string, double) result = age.HasValue && age.Value < Minimum
                ? (RatingLogStatus.Fail, 0.0)
                : (RatingLogStatus.Pass, Math.Round(Points / (double)totalPoints, 3));
            return [_ratingFactory.CreateLog(result.Item2, string.Format(CultureInfo.InvariantCulture, LogMessageTemplage, result.Item1, age, Minimum))];
        }
    }
}
