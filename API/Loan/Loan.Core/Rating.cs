using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class Rating : IRating
    {
        private readonly RatingData _data;
        private readonly IRatingDataSaver _dataSaver;
        private readonly List<IRatingLog> _logs;

        public Rating(RatingData data, IRatingDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
            _logs = data.RatingLogs != null ? data.RatingLogs.Select<RatingLogData, IRatingLog>(d => new RatingLog(d)).ToList() : new List<IRatingLog>();
        }

        public Guid RatingId => _data.RatingId;

        public double Value { get => _data.Value; set => _data.Value = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public ImmutableList<IRatingLog> RatingLogs => ImmutableList<IRatingLog>.Empty.AddRange(_logs);

        public async Task SaveLoanApplication(ITransactionHandler transactionHandler, Guid loanApplicationId)
        {
            _data.RatingLogs = new List<RatingLogData>();
            foreach (IRatingLog ratingLog in _logs)
            {
                if (ratingLog is RatingLog innerRatingLog)
                    _data.RatingLogs.Add(innerRatingLog.Data);
            }
            await _dataSaver.SaveLoanApplicationRating(transactionHandler, loanApplicationId, _data);
        }

        internal void AddLog(IRatingLog log)
            => _logs.Add(log);
    }
}
