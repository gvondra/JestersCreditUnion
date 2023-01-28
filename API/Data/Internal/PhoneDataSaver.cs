using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PhoneDataSaver : IPhoneDataSaver
    {
        private readonly ISqlDbProviderFactory _providerFactory;

        public PhoneDataSaver(ISqlDbProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }

        public async Task Create(ITransactionHandler transactionHandler, PhoneData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[jcu].[CreatePhone]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    id.Value = DataUtil.GetParameterValue(data.PhoneId);
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "number", DbType.AnsiString, DataUtil.GetParameterValue(data.Number));

                    await command.ExecuteNonQueryAsync();
                    data.PhoneId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }
    }
}
