using JestersCreditUnion.Loan.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentDataSaver : DataSaverBase, IPaymentDataSaver
    {
        private readonly IGenericDataFactory<PaymentData> _genericDataFactory;

        public PaymentDataSaver(IDbProviderFactory providerFactory, IGenericDataFactory<PaymentData> genericDataFactory)
            : base(providerFactory)
        {
            _genericDataFactory = genericDataFactory;
        }

        public async Task Save(ITransactionHandler transactionHandler, IEnumerable<PaymentData> payments)
        {
            await _providerFactory.EstablishTransaction(transactionHandler);
            foreach (PaymentData payment in payments)
            {
                PaymentData existingPayment = await GetByDateLoanIdTransactionNumber(
                    transactionHandler.Connection,
                    transactionHandler.Transaction.InnerTransaction,
                    payment.Date,
                    payment.LoanId,
                    payment.TransactionNumber);
                if (existingPayment == null)
                {
                    if (payment.PaymentId.Equals(Guid.Empty))
                        payment.PaymentId = Guid.NewGuid();
                    await Create(transactionHandler, payment);
                }
                else
                {
                    payment.PaymentId = existingPayment.PaymentId;
                    payment.Date = existingPayment.Date;
                    payment.LoanId = existingPayment.LoanId;
                    payment.TransactionNumber = existingPayment.TransactionNumber;
                    payment.UpdateTimestamp = existingPayment.UpdateTimestamp;
                    payment.CreateTimestamp = existingPayment.CreateTimestamp;
                    await Update(transactionHandler, payment);
                }
            }
        }

        private async Task Create(ITransactionHandler transactionHandler, PaymentData data)
        {
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[ln].[CreatePayment]";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                command.Parameters.Add(timestamp);

                DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.PaymentId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "loanId", DbType.Guid, DataUtil.GetParameterValue(data.LoanId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(data.TransactionNumber));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "date", DbType.Date, DataUtil.GetParameterValue(data.Date));
                AddCommonParameters(command.Parameters, data);

                await command.ExecuteNonQueryAsync();
                data.CreateTimestamp = (DateTime)timestamp.Value;
                data.UpdateTimestamp = (DateTime)timestamp.Value;
            }
        }

        public async Task Update(ITransactionHandler transactionHandler, PaymentData data)
        {
            await _providerFactory.EstablishTransaction(transactionHandler);
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[ln].[UpdatePayment]";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                command.Parameters.Add(timestamp);

                DataUtil.AddParameter(_providerFactory, command.Parameters, "id", DbType.Guid, DataUtil.GetParameterValue(data.PaymentId));
                AddCommonParameters(command.Parameters, data);

                await command.ExecuteNonQueryAsync();
                data.UpdateTimestamp = (DateTime)timestamp.Value;
            }
        }

        private static string GetByLoanIDateTransactionNumberdSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Payment.Select(c => $"`{Constants.TableName.Payment}`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Payment}` ");
            sql.Append("WHERE `LoanId` = @loanId AND `Date` = @date AND `TransactionNumber` = @transactionNumber ");
            sql.AppendLine("ORDER BY `Date`; ");

            return sql.ToString();
        }

        private void AddCommonParameters(IList commandParameters, PaymentData data)
        {
            DataUtil.AddParameter(_providerFactory, commandParameters, "amount", DbType.Decimal, DataUtil.GetParameterValue(data.Amount));
            DataUtil.AddParameter(_providerFactory, commandParameters, "status", DbType.Int16, DataUtil.GetParameterValue(data.Status));
        }

        private async Task<PaymentData> GetByDateLoanIdTransactionNumber(
            DbConnection connection,
            System.Data.Common.DbTransaction dbTransaction,
            DateTime date,
            Guid loanId,
            string transactionNumber)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = GetByLoanIDateTransactionNumberdSql();
                command.CommandType = CommandType.Text;
                command.Transaction = dbTransaction;
                DataUtil.AddParameter(_providerFactory, command.Parameters, "date", DbType.Date, DataUtil.GetParameterValue(date));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "loanId", DbType.Binary, DataUtil.GetParameterValueBinary(loanId));
                DataUtil.AddParameter(_providerFactory, command.Parameters, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(transactionNumber));

                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    return (await _genericDataFactory.LoadData(reader, () => new PaymentData(), DataUtil.AssignDataStateManager))
                        .FirstOrDefault();
                }
            }
        }
    }
}
