using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class TransactionDataSaver : DataSaverBase, ITransactionDataSaver
    {
        public TransactionDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory)
        { }

        public async Task Create(ITransactionHandler transactionHandler, TransactionData data, Guid? paymentId = null, short? termNumber = null)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "CreateTransaction";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Binary);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "loanId", DbType.Binary, DataUtil.GetParameterValueBinary(data.LoanId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "paymentId", DbType.Binary, DataUtil.GetParameterValueBinary(paymentId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "termNumber", DbType.Int16, DataUtil.GetParameterValue(termNumber));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.TransactionId = new Guid((byte[])id.Value);
                    data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
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
