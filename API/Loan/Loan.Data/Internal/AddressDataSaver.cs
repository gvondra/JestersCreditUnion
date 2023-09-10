using JestersCreditUnion.Data.Models;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System.Collections;

namespace JestersCreditUnion.Data.Internal
{
    public class AddressDataSaver : DataSaverBase, IAddressDataSaver
    {
        public AddressDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task Create(ISqlTransactionHandler transactionHandler, AddressData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateAddress]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.AddressId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, AddressData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "hash", DbType.Binary, DataUtil.GetParameterValue(data.Hash));
            DataUtil.AddParameter(_providerFactory, commandParameters, "recipient", DbType.String, DataUtil.GetParameterValue(data.Recipient));
            DataUtil.AddParameter(_providerFactory, commandParameters, "attention", DbType.String, DataUtil.GetParameterValue(data.Attention));
            DataUtil.AddParameter(_providerFactory, commandParameters, "delivery", DbType.String, DataUtil.GetParameterValue(data.Delivery));
            DataUtil.AddParameter(_providerFactory, commandParameters, "secondary", DbType.String, DataUtil.GetParameterValue(data.Secondary));
            DataUtil.AddParameter(_providerFactory, commandParameters, "city", DbType.String, DataUtil.GetParameterValue(data.City));
            DataUtil.AddParameter(_providerFactory, commandParameters, "state", DbType.String, DataUtil.GetParameterValue(data.State));
            DataUtil.AddParameter(_providerFactory, commandParameters, "postalCode", DbType.String, DataUtil.GetParameterValue(data.PostalCode));
        }
    }
}
