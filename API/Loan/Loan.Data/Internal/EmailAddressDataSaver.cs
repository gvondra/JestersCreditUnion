﻿using JestersCreditUnion.Loan.Data.Models;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class EmailAddressDataSaver : DataSaverBase, IEmailAddressDataSaver
    {
        public EmailAddressDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task Create(ISqlTransactionHandler transactionHandler, EmailAddressData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateEmailAddress]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "address", DbType.AnsiString, DataUtil.GetParameterValue(data.Address));

                    await command.ExecuteNonQueryAsync();
                    data.EmailAddressId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

    }
}
