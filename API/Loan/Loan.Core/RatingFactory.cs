using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class RatingFactory : IRatingFactory
    {
        private readonly IRatingDataFactory _dataFactory;
        private readonly IRatingDataSaver _dataSaver;

        public RatingFactory(IRatingDataFactory dataFactory, IRatingDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
        }

        private Rating Create(RatingData data) => new Rating(data, _dataSaver);

        public IRating Create(double value, IEnumerable<IRatingLog> ratingLogs)
        {
            Rating rating = Create(new RatingData());
            rating.Value = value;
            if (ratingLogs != null)
            {
                foreach (IRatingLog ratingLog in ratingLogs)
                {
                    rating.AddLog(ratingLog);
                }
            }
            return rating;
        }

        public IRatingLog CreateLog(double value, string description)
        {
            return new RatingLog(new RatingLogData())
            {
                Value = value,
                Description = (description ?? string.Empty).Trim()
            };
        }

        public async Task<IRating> GetLoanApplication(Framework.ISettings settings, Guid loanApplicationId)
        {
            RatingData data = await _dataFactory.GetByLoanApplicationId(new DataSettings(settings), loanApplicationId);
            return data != null ? Create(data) : null;
        }
    }
}
