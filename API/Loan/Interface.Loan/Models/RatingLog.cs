using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class RatingLog
    {
        public double? Value { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTimestamp { get; set; }
    }
}
