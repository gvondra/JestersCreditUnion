using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentIntakeDataSaver : DataSaverBase, IPaymentIntakeDataSaver
    {
        public PaymentIntakeDataSaver(IDbProviderFactory providerFactory) : base(providerFactory)
        { }

        public async Task Create(ITransactionHandler transactionHandler, PaymentIntakeData data, string userId)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreatePaymentIntake]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "loanId", DbType.Guid, DataUtil.GetParameterValue(data.LoanId));
                    AddCommonParameters(command.Parameters, data, userId);

                    await command.ExecuteNonQueryAsync();
                    data.PaymentIntakeId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        public async Task Update(ITransactionHandler transactionHandler, PaymentIntakeData data, string userId)
        {
            if (data.Manager.GetState(data) == DataState.Updated)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[UpdatePaymentIntake]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.PaymentIntakeId));
                    AddCommonParameters(command.Parameters, data, userId);

                    await command.ExecuteNonQueryAsync();
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, PaymentIntakeData data, string userId)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "paymentId", DbType.Guid, DataUtil.GetParameterValue(data.PaymentId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(data.TransactionNumber));
            DataUtil.AddParameter(_providerFactory, commandParameters, "date", DbType.Date, DataUtil.GetParameterValue(data.Date));
            DataUtil.AddParameter(_providerFactory, commandParameters, "amount", DbType.Decimal, DataUtil.GetParameterValue(data.Amount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "status", DbType.Int16, DataUtil.GetParameterValue(data.Status));
            DataUtil.AddParameter(_providerFactory, commandParameters, "userId", DbType.AnsiString, DataUtil.GetParameterValue(userId));
        }
    }
}
