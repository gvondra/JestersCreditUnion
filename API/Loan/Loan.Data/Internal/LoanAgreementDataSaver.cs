using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
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
                    command.CommandText = "CreateLoanAgreement";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Binary, DataUtil.GetParameterValueBinary(data.LoanId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
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
                    command.CommandText = "UpdateLoanAgreement";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Binary, DataUtil.GetParameterValueBinary(data.LoanId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
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
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmailAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerPhoneId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerName", DbType.String, DataUtil.GetParameterValue(data.CoBorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.CoBorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.CoBorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmailAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.CoBorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerPhoneId", DbType.Binary, DataUtil.GetParameterValueBinary(data.CoBorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "originalAmount", DbType.Decimal, DataUtil.GetParameterValue(data.OriginalAmount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "originalTerm", DbType.Int16, DataUtil.GetParameterValue(data.OriginalTerm));
            DataUtil.AddParameter(_providerFactory, commandParameters, "interestRate", DbType.Decimal, DataUtil.GetParameterValue(data.InterestRate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "paymentAmount", DbType.Decimal, DataUtil.GetParameterValue(data.PaymentAmount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "paymentFrequency", DbType.Int16, DataUtil.GetParameterValue(data.PaymentFrequency));
        }
    }
}
