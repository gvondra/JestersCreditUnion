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

        public async Task AppendComment(ITransactionHandler transactionHandler, LoanApplicationCommentData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "CreateLoanApplicationComment";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Binary);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "loanApplicationId", DbType.Binary, DataUtil.GetParameterValueBinary(data.LoanApplicationId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "userId", DbType.Binary, DataUtil.GetParameterValueBinary(data.UserId));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "isInternal", DbType.Boolean, DataUtil.GetParameterValue(data.IsInternal));
                    DataUtil.AddParameter(_providerFactory, command.Parameters, "text", DbType.String, DataUtil.GetParameterValue(data.Text));

                    await command.ExecuteNonQueryAsync();
                    data.LoanApplicationCommentId = new Guid((byte[])id.Value);
                    data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                }
            }
        }

        public async Task SetDenial(ITransactionHandler transactionHandler, Guid id, short loanApplicationStatus, DateTime? closedDate, LoanApplicationDenialData denial)
        {
            await _providerFactory.EstablishTransaction(transactionHandler, denial);
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "SetLoanApplicationDenial_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                timestamp.Direction = ParameterDirection.Output;
                command.Parameters.Add(timestamp);

                DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Binary, DataUtil.GetParameterValueBinary(id));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "status", DbType.Int16, DataUtil.GetParameterValue(loanApplicationStatus));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "closedDate", DbType.Date, DataUtil.GetParameterValue(closedDate));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "userId", DbType.Binary, DataUtil.GetParameterValueBinary(denial.UserId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "reason", DbType.Int16, DataUtil.GetParameterValue(denial.Reason));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "date", DbType.Date, DataUtil.GetParameterValue(denial.Date));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "text", DbType.String, DataUtil.GetParameterValue(denial.Text));

                await command.ExecuteNonQueryAsync();
                denial.LoanApplicationId = id;
                denial.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                denial.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
            }
        }

        public async Task Create(ITransactionHandler transactionHandler, LoanApplicationData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "CreateLoanApplication";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Binary);
                    id.Direction = ParameterDirection.Output;
                    command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "userID", DbType.Binary, DataUtil.GetParameterValueBinary(data.UserId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.LoanApplicationId = new Guid((byte[])id.Value);
                    data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                }
            }
        }

        public async Task Update(ITransactionHandler transactionHandler, LoanApplicationData data)
        {
            if (data.Manager.GetState(data) == DataState.Updated)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "UpdateLoanApplication";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime);
                    timestamp.Direction = ParameterDirection.Output;
                    command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Binary, DataUtil.GetParameterValueBinary(data.LoanApplicationId));
                    AddCommonParameters(command.Parameters, data);

                    await command.ExecuteNonQueryAsync();
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, LoanApplicationData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "status", DbType.Int16, DataUtil.GetParameterValue(data.Status));
            DataUtil.AddParameter(_providerFactory, commandParameters, "applicationDate", DbType.Date, DataUtil.GetParameterValue(data.ApplicationDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerName", DbType.String, DataUtil.GetParameterValue(data.BorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.BorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmailAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerPhoneId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmployerName", DbType.String, DataUtil.GetParameterValue(data.BorrowerEmployerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerEmploymentHireDate", DbType.Date, DataUtil.GetParameterValue(data.BorrowerEmploymentHireDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerIncome", DbType.Decimal, DataUtil.GetParameterValue(data.BorrowerIncome));
            DataUtil.AddParameter(_providerFactory, commandParameters, "borrowerIdentificationCardId", DbType.Binary, DataUtil.GetParameterValueBinary(data.BorrowerIdentificationCardId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerName", DbType.String, DataUtil.GetParameterValue(data.CoBorrowerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerBirthDate", DbType.Date, DataUtil.GetParameterValue(data.CoBorrowerBirthDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.CoBorrowerAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmailAddressId", DbType.Binary, DataUtil.GetParameterValueBinary(data.CoBorrowerEmailAddressId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerPhoneId", DbType.Binary, DataUtil.GetParameterValueBinary(data.CoBorrowerPhoneId));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmployerName", DbType.String, DataUtil.GetParameterValue(data.CoBorrowerEmployerName));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerEmploymentHireDate", DbType.Date, DataUtil.GetParameterValue(data.CoBorrowerEmploymentHireDate));
            DataUtil.AddParameter(_providerFactory, commandParameters, "coBorrowerIncome", DbType.Decimal, DataUtil.GetParameterValue(data.CoBorrowerIncome));
            DataUtil.AddParameter(_providerFactory, commandParameters, "amount", DbType.Decimal, DataUtil.GetParameterValue(data.Amount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "purpose", DbType.String, DataUtil.GetParameterValue(data.Purpose));
            DataUtil.AddParameter(_providerFactory, commandParameters, "mortgagePayment", DbType.Decimal, DataUtil.GetParameterValue(data.MortgagePayment));
            DataUtil.AddParameter(_providerFactory, commandParameters, "rentPayment", DbType.Decimal, DataUtil.GetParameterValue(data.RentPayment));
            DataUtil.AddParameter(_providerFactory, commandParameters, "closedDate", DbType.Date, DataUtil.GetParameterValue(data.ClosedDate));
        }
    }
}
