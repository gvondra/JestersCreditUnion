using JestersCreditUnion.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = BrassLoon.Interface.Config.Models;

namespace JestersCreditUnion.Core
{
    public class Lookup : ILookup
    {
        private readonly Models.Lookup _data;

        internal Lookup(Models.Lookup lookup)
        {
            _data = lookup;
        }

        public string Code { get => _data.Code; set => _data.Code = value; }
        public Dictionary<string, string> Data { get => _data.Data ?? new Dictionary<string, string>(); set => _data.Data = value; }
        public DateTime CreateTimestamp { get => _data.CreateTimestamp ?? DateTime.UtcNow; set => _data.CreateTimestamp = value; }
        public DateTime UpdateTimestamp { get => _data.UpdateTimestamp ?? DateTime.UtcNow; set => _data.UpdateTimestamp = value; }

        public string GetDataValue(object key)
        {
            string result = null;
            if (!typeof(string).Equals(key.GetType()))
            {
                if (key.GetType().IsEnum)
                    key = Convert.ToInt32(key).ToString();
                else
                    key = key.ToString();
            }
            if (Data.ContainsKey((string)key))
                result = Data[(string)key];
            return result;
        }
    }
}
