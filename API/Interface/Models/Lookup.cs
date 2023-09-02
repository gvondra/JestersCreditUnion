using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Interface.Models
{
    public class Lookup
    {
        public string Code { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public DateTime? CreateTimestamp { get; set; }

        public DateTime? UpdateTimestamp { get; set; }
    }
}
