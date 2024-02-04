using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Data.Models
{
    public class RatingData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid RatingId { get; set; }
        [ColumnMapping] public double Value { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }

        public List<RatingLogData> RatingLogs { get; set; } = new List<RatingLogData>();
    }
}
