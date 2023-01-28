using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class AddressDataSaver : IAddressDataSaver
    {
        private readonly ISqlDbProviderFactory _providerFactory;

        public AddressDataSaver(ISqlDbProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }

        public async Task Create(ITransactionHandler transactionHandler, AddressData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[jcu].[CreateAddress]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    id.Value = DataUtil.GetParameterValue(data.AddressId);
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "hash", DbType.Binary, DataUtil.GetParameterValue(data.Hash));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "recipient", DbType.AnsiString, DataUtil.GetParameterValue(data.Recipient));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "attention", DbType.AnsiString, DataUtil.GetParameterValue(data.Attention));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "delivery", DbType.AnsiString, DataUtil.GetParameterValue(data.Delivery));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "secondary", DbType.AnsiString, DataUtil.GetParameterValue(data.Secondary));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "city", DbType.AnsiString, DataUtil.GetParameterValue(data.City));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "state", DbType.AnsiString, DataUtil.GetParameterValue(data.State));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "postalCode", DbType.AnsiString, DataUtil.GetParameterValue(data.PostalCode));

                    await command.ExecuteNonQueryAsync();
                    data.AddressId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }
    }
}
