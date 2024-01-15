using System;
using System.Collections.Immutable;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IRating
    {
        Guid RatingId { get; }
        double Value { get; set; }
        DateTime CreateTimestamp { get; }

        ImmutableList<IRatingLog> RatingLogs { get; }
    }
}
