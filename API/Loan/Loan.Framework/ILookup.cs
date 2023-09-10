using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILookup
    {
        string Code { get; set; }

        Dictionary<string, string> Data { get; set; }

        public DateTime CreateTimestamp { get; set; }

        public DateTime UpdateTimestamp { get; set; }

        string GetDataValue(object key);
    }
}
