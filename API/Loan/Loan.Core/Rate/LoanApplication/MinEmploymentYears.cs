using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace JestersCreditUnion.Loan.Core.Rate.LoanApplication
{
    internal class MinEmploymentYears : IComponent
    {
        private readonly IRatingFactory _ratingFactory;

        public MinEmploymentYears(IRatingFactory ratingFactory, int points)
        {
            _ratingFactory = ratingFactory;
            Points = points;
        }

        public int Points { get; set; }
        internal int Minimum { get; set; }
        internal string LogMessageTemplage { get; set; }
        internal bool BorrowerEmployment { get; set; }
        internal bool CoBorrowerEmployment { get; set; }

        public IEnumerable<IRatingLog> Evaluate(ILoanApplication loanApplication, int totalPoints)
        {
            int? age;
            if (BorrowerEmployment)
                age = loanApplication.GetBorrowerEmploymentYears() ?? 0;
            else if (CoBorrowerEmployment)
                age = loanApplication.GetCoBorrowerEmploymentYears() ?? (loanApplication.HasCoBorrower() ? 0 : null);
            else
                age = loanApplication.GetBorrowerEmploymentYears() ?? 0;

            (string, double) result = age.HasValue && age.Value < Minimum
                ? (RatingLogStatus.Fail, 0.0)
                : (RatingLogStatus.Pass, Math.Round(Points / (double)totalPoints, 3));
            return [_ratingFactory.CreateLog(result.Item2, string.Format(CultureInfo.InvariantCulture, LogMessageTemplage, result.Item1, age, Minimum))];
        }
    }
}
