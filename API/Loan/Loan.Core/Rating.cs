using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace JestersCreditUnion.Loan.Core
{
    public class Rating : IRating
    {
        private readonly RatingData _data;
        private readonly List<IRatingLog> _logs;

        public Rating(RatingData data)
        {
            _data = data;
            _logs = data.RatingLogs != null ? data.RatingLogs.Select<RatingLogData, IRatingLog>(d => new RatingLog(d)).ToList() : new List<IRatingLog>();
        }

        public Guid RatingId => _data.RatingId;

        public double Value { get => _data.Value; set => _data.Value = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public ImmutableList<IRatingLog> RatingLogs => ImmutableList<IRatingLog>.Empty.AddRange(_logs);

        internal void AddLog(IRatingLog log)
            =>_logs.Add(log);
    }
}
