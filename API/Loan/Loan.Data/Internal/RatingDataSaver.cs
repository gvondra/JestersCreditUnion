using JestersCreditUnion.Loan.Data.Models;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class RatingDataSaver : DataSaverBase, IRatingDataSaver
    {
        public RatingDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory)
        { }

        public async Task SaveLoanApplicationRating(ITransactionHandler transactionHandler, Guid loanApplicationId, RatingData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                await InnerSaveLoanApplicationRating(transactionHandler, loanApplicationId, data);
                if (data.RatingLogs != null)
                {
                    foreach (RatingLogData ratingLog in data.RatingLogs
                        .Where(rl => rl.Manager.GetState(rl) == DataState.New))
                    {
                        ratingLog.RatingId = data.RatingId;
                        await CreateRatingLog(transactionHandler, ratingLog);
                    }
                }
            }
        }

        private async Task InnerSaveLoanApplicationRating(ITransactionHandler transactionHandler, Guid loanApplicationId, RatingData data)
        {
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[ln].[CreateLoanApplicationRating]";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                id.Direction = ParameterDirection.Output;
                command.Parameters.Add(id);

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                command.Parameters.Add(timestamp);

                DataUtil.AddParameter(_providerFactory, command.Parameters, "loanApplicationId", DbType.Guid, DataUtil.GetParameterValue(loanApplicationId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "value", DbType.Double, DataUtil.GetParameterValue(data.Value));

                await command.ExecuteNonQueryAsync();
                data.RatingId = (Guid)id.Value;
                data.CreateTimestamp = (DateTime)timestamp.Value;
            }
        }

        private async Task CreateRatingLog(ITransactionHandler transactionHandler, RatingLogData data)
        {
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[ln].[CreateRatingLog]";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                id.Direction = ParameterDirection.Output;
                command.Parameters.Add(id);

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                command.Parameters.Add(timestamp);

                DataUtil.AddParameter(_providerFactory, command.Parameters, "ratingId", DbType.Guid, DataUtil.GetParameterValue(data.RatingId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "value", DbType.Double, DataUtil.GetParameterValue(data.Value));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "description", DbType.AnsiString, DataUtil.GetParameterValue(data.Description));

                await command.ExecuteNonQueryAsync();
                data.RatingLogId = (Guid)id.Value;
                data.CreateTimestamp = (DateTime)timestamp.Value;
            }
        }
    }
}
