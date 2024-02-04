using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public double? Value { get; set; }
        public DateTime? CreateTimestamp { get; set; }

        public List<RatingLog> RatingLogs { get; set; }
    }
}
