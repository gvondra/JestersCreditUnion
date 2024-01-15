using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;

namespace JestersCreditUnion.Loan.Core
{
    public class RatingLog : IRatingLog
    {
        private readonly RatingLogData _data;

        public RatingLog(RatingLogData data)
        {
            _data = data;
        }

        public double? Value { get => _data.Value; set => _data.Value = value; }
        public string Description { get => _data.Description; set => _data.Description = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;
    }
}
