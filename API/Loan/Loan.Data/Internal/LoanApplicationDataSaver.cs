using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class LoanApplicationDataSaver : DataSaverBase, ILoanApplicationDataSaver
    {
        public LoanApplicationDataSaver(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task AppendComment(ISqlTransactionHandler transactionHandler, LoanApplicationCommentData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateLoanApplicationComment]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "loanApplicationId", DbType.Guid, DataUtil.GetParameterValue(data.LoanApplicationId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "userId", DbType.Guid, DataUtil.GetParameterValue(data.UserId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "isInternal", DbType.Boolean, DataUtil.GetParameterValue(data.IsInternal));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "text", DbType.String, DataUtil.GetParameterValue(data.Text));

                    await command.ExecuteNonQueryAsync();
                    data.LoanApplicationId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        public async Task SetDenial(ISqlTransactionHandler transactionHandler, Guid id, short loanApplicationStatus, LoanApplicationDenialData denial)
        {
            await _providerFactory.EstablishTransaction(transactionHandler, denial);
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[ln].[SetLoanApplicationDenial]";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                command.Parameters.Add(timestamp);

                DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(id));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "status", DbType.Int16, DataUtil.GetParameterValue(loanApplicationStatus));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "userId", DbType.Guid, DataUtil.GetParameterValue(denial.UserId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "reason", DbType.Int16, DataUtil.GetParameterValue(denial.Reason));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "date", DbType.Date, DataUtil.GetParameterValue(denial.Date));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "text", DbType.String, DataUtil.GetParameterValue(denial.Text));

                await command.ExecuteNonQueryAsync();
                denial.LoanApplicationId = id;
                denial.CreateTimestamp = (DateTime)timestamp.Value;
                denial.UpdateTimestamp = (DateTime)timestamp.Value;
            }
        }

        public async Task Create(ISqlTransactionHandler transactionHandler, LoanApplicationData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[CreateLoanApplication]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "userID", DbType.Guid, DataUtil.GetParameterValue(data.UserId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.LoanApplicationId = (Guid)id.Value;
                    data.CreateTimestamp = (DateTime)timestamp.Value;
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        public async Task Update(ISqlTransactionHandler transactionHandler, LoanApplicationData data)
        {
            if (data.Manager.GetState(data) == DataState.Updated)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[ln].[UpdateLoanApplication]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.LoanApplicationId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.UpdateTimestamp = (DateTime)timestamp.Value;
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, LoanApplicationData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "status", DbType.Int16, DataUtil.GetParameterValue(data.Status));
            DataUtil.AddParameter(_providerFactory, commandParameters, "applicationDate", DbType.Date, DataUtil.GetParameterValue(data.ApplicationDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerName", DbType.String, DataUtil.GetParameterValue(data.BorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.BorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerAddressId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmailAddressId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerPhoneId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmployerName", DbType.String, DataUtil.GetParameterValue(data.BorrowerEmployerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmploymentHireDate", DbType.Date, DataUtil.GetParameterValue(data.BorrowerEmploymentHireDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerIncome", DbType.Decimal, DataUtil.GetParameterValue(data.BorrowerIncome));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerIdentificationCardId", DbType.Guid, DataUtil.GetParameterValue(data.BorrowerIdentificationCardId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerName", DbType.String, DataUtil.GetParameterValue(data.CoBorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.CoBorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerAddressId", DbType.Guid, DataUtil.GetParameterValue(data.CoBorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmailAddressId", DbType.Guid, DataUtil.GetParameterValue(data.CoBorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerPhoneId", DbType.Guid, DataUtil.GetParameterValue(data.CoBorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmployerName", DbType.String, DataUtil.GetParameterValue(data.CoBorrowerEmployerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmploymentHireDate", DbType.Date, DataUtil.GetParameterValue(data.CoBorrowerEmploymentHireDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerIncome", DbType.Decimal, DataUtil.GetParameterValue(data.CoBorrowerIncome));
            DataUtil.AddParameter(_providerFactory, commandParameters, "amount", DbType.Decimal, DataUtil.GetParameterValue(data.Amount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "purpose", DbType.String, DataUtil.GetParameterValue(data.Purpose));
            DataUtil.AddParameter(_providerFactory, commandParameters, "mortgagePayment", DbType.Decimal, DataUtil.GetParameterValue(data.MortgagePayment));
            DataUtil.AddParameter(_providerFactory, commandParameters, "rentPayment", DbType.Decimal, DataUtil.GetParameterValue(data.RentPayment));
        }
    }
}
