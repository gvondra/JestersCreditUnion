using JestersCreditUnion.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class TransactionDataSaver : DataSaverBase, ITransactionDataSaver
    {
        public TransactionDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory)
        { }

        public async Task Create(ISqlTransactionHandler transactionHandler, TransactionData data, Guid? paymentId = null)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateTransaction]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "loanId", DbType.Guid, DataUtil.GetParameterValue(data.LoanId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "paymentId", DbType.Guid, DataUtil.GetParameterValue(paymentId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.TransactionId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, TransactionData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "date", DbType.Date, DataUtil.GetParameterValue(data.Date));
            DataUtil.AddParameter(_providerFactory, commandParameters, "type", DbType.Int16, DataUtil.GetParameterValue(data.Type));
            DataUtil.AddParameter(_providerFactory, commandParameters, "amount", DbType.Decimal, DataUtil.GetParameterValue(data.Amount));
        }
    }
}
