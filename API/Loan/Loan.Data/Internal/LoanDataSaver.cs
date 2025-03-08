using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class LoanDataSaver : DataSaverBase, ILoanDataSaver
    {
        public LoanDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public ILoanAgreementDataSaver LoanAgrementDataSaver { get; set; }

        public async Task Create(ITransactionHandler transactionHandler, LoanData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateLoan]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.LoanId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "number", DbType.AnsiString, DataUtil.GetParameterValue(data.Number));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "loanApplicationId", DbType.Guid, DataUtil.GetParameterValue(data.LoanApplicationId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        public async Task Update(ITransactionHandler transactionHandler, LoanData data)
        {
            if (data.Manager.GetState(data) == DataState.Updated)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[UpdateLoan]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.LoanId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, LoanData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "initialDisbursementDate", DbType.Date, DataUtil.GetParameterValue(data.InitialDisbursementDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "firstPaymentDue", DbType.Date, DataUtil.GetParameterValue(data.FirstPaymentDue));
            DataUtil.AddParameter(_providerFactory, commandParameters, "nextPaymentDue", DbType.Date, DataUtil.GetParameterValue(data.NextPaymentDue));
            DataUtil.AddParameter(_providerFactory, commandParameters, "status", DbType.Int16, DataUtil.GetParameterValue(data.Status));
            DataUtil.AddParameter(_providerFactory, commandParameters, "balance", DbType.Decimal, DataUtil.GetParameterValue(data.Balance));
        }
    }
}
