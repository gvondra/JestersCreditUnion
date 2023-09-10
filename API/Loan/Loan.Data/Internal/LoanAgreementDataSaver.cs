using JestersCreditUnion.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanAgreementDataSaver : DataSaverBase, ILoanAgreementDataSaver
    {
        public LoanAgreementDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task Create(ITransactionHandler transactionHandler, LoanAgreementData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateLoanAgreement]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.LoanId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        public async Task Update(ITransactionHandler transactionHandler, LoanAgreementData data)
        {
            if (data.Manager.GetState(data) == DataState.Updated)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[UpdateLoanAgreement]";
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

        private void AddCommonParameters(IList commandParameters, LoanAgreementData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "status", DbType.Int16, DataUtil.GetParameterValue(data.Status));
            DataUtil.AddParameter(_providerFactory, commandParameters, "createDate", DbType.Date, DataUtil.GetParameterValue(data.CreateDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "agreementDate", DbType.Date, DataUtil.GetParameterValue(data.AgreementDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerName", DbType.String, DataUtil.GetParameterValue(data.BorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.BorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerAddressId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmailAddressId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerPhoneId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerName", DbType.String, DataUtil.GetParameterValue(data.CoBorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.CoBorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerAddressId", DbType.Guid, DataUtil.GetParameterValue(data.CoBorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmailAddressId", DbType.Guid, DataUtil.GetParameterValue(data.CoBorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerPhoneId", DbType.Guid, DataUtil.GetParameterValue(data.CoBorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "originalAmount", DbType.Decimal, DataUtil.GetParameterValue(data.OriginalAmount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "originalTerm", DbType.Int16, DataUtil.GetParameterValue(data.OriginalTerm));
            DataUtil.AddParameter(_providerFactory, commandParameters, "interestRate", DbType.Decimal, DataUtil.GetParameterValue(data.InterestRate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "paymentAmount", DbType.Decimal, DataUtil.GetParameterValue(data.PaymentAmount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "paymentFrequency", DbType.Int16, DataUtil.GetParameterValue(data.PaymentFrequency));
        }
    }
}
