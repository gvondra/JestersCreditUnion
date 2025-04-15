using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentDataFactory : DataFactoryBase<PaymentData>, IPaymentDataFactory
    {
        private readonly IGenericDataFactory<PaymentTransactionData> _paymentTransactionDataFactory;

        public PaymentDataFactory(
            IDbProviderFactory providerFactory,
            IGenericDataFactory<PaymentData> dataFactory,
            IGenericDataFactory<PaymentTransactionData> paymentTransactionDataFactory)
            : base(providerFactory, dataFactory)
        {
            _paymentTransactionDataFactory = paymentTransactionDataFactory;
        }

        public async Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISettings settings, DateTime date, Guid loanId, string transactionNumber)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "date", DbType.Date, DataUtil.GetParameterValue(date)),
                DataUtil.CreateParameter(ProviderFactory, "loanId", DbType.Binary, DataUtil.GetParameterValueBinary(loanId)),
                DataUtil.CreateParameter(ProviderFactory, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(transactionNumber))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            IEnumerable<PaymentData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByDateLoanNumberTransactionNumberSql(),
                CommandType.Text,
                parameters,
                Read);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<PaymentData>> GetByLoanId(ISettings settings, Guid loanId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "loanId", DbType.Binary, DataUtil.GetParameterValueBinary(loanId))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            return await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByLoanIdSql(),
                CommandType.Text,
                parameters,
                Read);
        }

        private static string GetByDateLoanNumberTransactionNumberSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Payment.Select(c => $"`{Constants.TableName.Payment}`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Payment}` ");
            sql.Append("WHERE `Date` = @date ");
            sql.Append("AND `LoanId` = @loanId ");
            sql.AppendLine("AND `TransactionNumber` = @transactionNumber ");
            sql.AppendLine("ORDER BY `Date`; ");

            sql.Append($"SELECT `{Constants.TableName.PaymentTransaction}`.`PaymentId`, `{Constants.TableName.PaymentTransaction}`.`TermNumber`, ");
            sql.AppendLine(string.Join(", ", Constants.Column.Transaction.Select(c => $"`{Constants.TableName.Transaction}`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Transaction}` ");
            sql.AppendLine($"INNER JOIN `{Constants.TableName.PaymentTransaction}` ON `{Constants.TableName.Transaction}`.`TransactionId` = `{Constants.TableName.PaymentTransaction}`.`TransactionId` ");
            sql.Append($"WHERE EXISTS (SELECT 1 FROM `{Constants.TableName.Payment}` `pmt` ");
            sql.Append($"WHERE `pmt`.`PaymentId` = `{Constants.TableName.PaymentTransaction}`.`PaymentId` ");
            sql.Append("AND `pmt`.`Date` = @date ");
            sql.Append("AND `pmt`.`LoanId` = @loanId ");
            sql.AppendLine("AND `pmt`.`TransactionNumber` = @transactionNumber ");
            sql.AppendLine("LIMIT 1); ");

            return sql.ToString();
        }

        private static string GetByLoanIdSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Payment.Select(c => $"`{Constants.TableName.Payment}`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Payment}` ");
            sql.Append("WHERE `LoanId` = @loanId ");
            sql.AppendLine("ORDER BY `Date`; ");

            sql.Append($"SELECT `{Constants.TableName.PaymentTransaction}`.`PaymentId`, `{Constants.TableName.PaymentTransaction}`.`TermNumber`, ");
            sql.AppendLine(string.Join(", ", Constants.Column.Transaction.Select(c => $"`{Constants.TableName.Transaction}`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Transaction}` ");
            sql.AppendLine($"INNER JOIN `{Constants.TableName.PaymentTransaction}` ON `{Constants.TableName.Transaction}`.`TransactionId` = `{Constants.TableName.PaymentTransaction}`.`TransactionId` ");
            sql.Append($"WHERE EXISTS (SELECT 1 FROM `{Constants.TableName.Payment}` `pmt` ");
            sql.Append($"WHERE `pmt`.`PaymentId` = `{Constants.TableName.PaymentTransaction}`.`PaymentId` ");
            sql.AppendLine("AND `pmt`.`LoanId` = @loanId ");
            sql.AppendLine("LIMIT 1); ");

            return sql.ToString();
        }

        private async Task<IEnumerable<PaymentData>> Read(DbDataReader reader)
        {
            IEnumerable<PaymentData> result = await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager);
            IEnumerable<PaymentTransactionData> transactions = Enumerable.Empty<PaymentTransactionData>();
            if (await reader.NextResultAsync())
                transactions = await _paymentTransactionDataFactory.LoadData(reader, () => new PaymentTransactionData(), DataUtil.AssignDataStateManager);
            result = result.GroupJoin(
                transactions,
                pmt => pmt.PaymentId,
                trx => trx.PaymentId,
                (pmt, trx) =>
                {
                    pmt.Transactions = trx.ToList();
                    return pmt;
                });
            return result.ToList();
        }
    }
}
