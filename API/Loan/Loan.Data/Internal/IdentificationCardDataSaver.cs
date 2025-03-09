using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class IdentificationCardDataSaver : DataSaverBase, IIdentificationCardDataSaver
    {
        public IdentificationCardDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task Create(ITransactionHandler transactionHandler, IdentificationCardData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "CreateIdentificationCard";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                }
            }
        }

        public async Task Update(ITransactionHandler transactionHandler, IdentificationCardData data)
        {
            if (data.Manager.GetState(data) == DataState.Updated)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "UpdateIdentificationCard";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, IdentificationCardData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "id", DbType.Binary, DataUtil.GetParameterValueBinary(data.IdentificationCardId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "initializationVector", DbType.Binary, DataUtil.GetParameterValue(data.InitializationVector));
            DataUtil.AddParameter(_providerFactory, commandParameters, "key", DbType.Binary, DataUtil.GetParameterValue(data.Key));
            DataUtil.AddParameter(_providerFactory, commandParameters, "masterKeyName", DbType.AnsiString, DataUtil.GetParameterValue(data.MasterKeyName));
        }
    }
}
