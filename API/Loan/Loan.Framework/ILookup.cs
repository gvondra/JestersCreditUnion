using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILookup
    {
        string Code { get; set; }

        Dictionary<string, string> Data { get; set; }

        DateTime CreateTimestamp { get; set; }

        DateTime UpdateTimestamp { get; set; }

        string GetDataValue(object key);
    }
}
