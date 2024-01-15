using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Core
{
    public class RatingFactory : IRatingFactory
    {
        private static Rating Create(RatingData data) => new Rating(data);

        public IRating Create(double value, IEnumerable<IRatingLog> ratingLogs)
        {
            Rating rating = Create(new RatingData());
            rating.Value = value;
            if (ratingLogs != null)
            {
                foreach (IRatingLog ratingLog in ratingLogs)
                {
                    rating.AddLog(ratingLog);
                }
            }
            return rating;
        }

        public IRatingLog CreateLog(double value, string description)
        {
            return new RatingLog(new RatingLogData())
            {
                Value = value,
                Description = (description ?? string.Empty).Trim()
            };
        }
    }
}
