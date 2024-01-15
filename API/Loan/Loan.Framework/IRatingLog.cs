using System;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IRatingLog
    {
        double? Value { get; set; }
        string Description { get; set; }
        DateTime CreateTimestamp { get; }
    }
}
