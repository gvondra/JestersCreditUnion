using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IRatingFactory
    {
        IRating Create(double value, IEnumerable<IRatingLog> ratingLogs);
        IRatingLog CreateLog(double value, string description);
    }
}
