using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JestersCreditUnion.Loan.Data.Models;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class LoanDataFactory : DataFactoryBase<LoanData>, ILoanDataFactory
    {
        public LoanDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<LoanData> dataFactory)
            : base(providerFactory, dataFactory) { }

        public async Task<LoanData> Get(ISettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "id", DbType.Binary, DataUtil.GetParameterValueBinary(id))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            List<LoanData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
            return result.FirstOrDefault();
        }

        public async Task<LoanData> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "loanApplicationId", DbType.Binary, DataUtil.GetParameterValueBinary(loanApplicationId))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            List<LoanData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByLoanApplicationIdSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<LoanData>> GetByNameBirthDate(ISettings settings, string name, DateTime birthDate)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            name = string.Format(
                CultureInfo.InvariantCulture,
                "%{0}%",
                name.Replace(@"\", @"\\").Replace(@"_", @"\_").Replace(@"%", @"\%"));
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "name", DbType.String, DataUtil.GetParameterValue(name)),
                DataUtil.CreateParameter(ProviderFactory, "birthDate", DbType.Date, DataUtil.GetParameterValue(birthDate))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            return await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByNameBirthDateSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
        }

        public async Task<LoanData> GetByNumber(ISettings settings, string number)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "number", DbType.AnsiString, DataUtil.GetParameterValue(number))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            List<LoanData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByNumberSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<LoanData>> GetWithUnprocessedPayments(ISettings settings)
        {
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            return await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetWithUnprocessedPaymentsSql(),
                commandType: CommandType.Text,
                parameters: Array.Empty<IDataParameter>(),
                Read);
        }

        private static string GetSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Loan.Select(c => $"`ln`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.AppendLine("WHERE `LoanId` = @id ");
            sql.AppendLine("LIMIT 1; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanAgreement.Select(c => $"`lagmt`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.LoanAgreement}` `lagmt` ");
            sql.AppendLine("WHERE `LoanId` = @id ");
            sql.AppendLine("LIMIT 1; ");

            return sql.ToString();
        }

        private static string GetByLoanApplicationIdSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Loan.Select(c => $"`ln`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.AppendLine("WHERE `LoanApplicationId` = @loanApplicationId ");
            sql.AppendLine("LIMIT 1; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanAgreement.Select(c => $"`lagmt`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.LoanAgreement}` `lagmt` ");
            sql.Append("WHERE EXISTS (SELECT 1 ");
            sql.Append($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.Append("WHERE `ln`.`LoanId` = `lagmt`.`LoanId` ");
            sql.Append("AND `ln`.`LoanApplicationId` = @loanApplicationId ");
            sql.AppendLine("LIMIT 1); ");

            return sql.ToString();
        }

        private static string GetByNameBirthDateSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Loan.Select(c => $"`ln`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.Append("WHERE EXISTS (SELECT 1 ");
            sql.Append($"FROM `{Constants.TableName.LoanAgreement}` `lagmt` ");
            sql.Append("WHERE `lagmt`.`LoanId` = `ln`.`LoanId` ");
            sql.Append("AND ((`lagmt`.`BorrowerName` LIKE @name ESCAPE '\' AND `lagmt`.`BorrowerBirthDate = @birthDate) ");
            sql.Append("OR (`lagmt`.`CoBorrowerName` LIKE @name ESCAPE '\' AND `lagmt`.`CoBorrowerBirthDate = @birthDate)) ");
            sql.AppendLine("LIMIT 1); ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanAgreement.Select(c => $"`lagmt`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.LoanAgreement}` `lagmt` ");
            sql.Append("WHERE (`lagmt`.`BorrowerName` LIKE @name ESCAPE '\' AND `lagmt`.`BorrowerBirthDate = @birthDate) ");
            sql.Append("OR (`lagmt`.`CoBorrowerName` LIKE @name ESCAPE '\' AND `lagmt`.`CoBorrowerBirthDate = @birthDate); ");

            return sql.ToString();
        }

        private static string GetByNumberSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Loan.Select(c => $"`ln`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.AppendLine("WHERE `ln`.`Number` = @number; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanAgreement.Select(c => $"`lagmt`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.LoanAgreement}` `lagmt` ");
            sql.Append("WHERE EXISTS (SELECT 1 ");
            sql.Append($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.Append("WHERE `ln`.`LoanId` = `lagmt`.`LoanId` ");
            sql.Append("AND `ln`.`Number` = @number ");
            sql.AppendLine("LIMIT 1); ");

            return sql.ToString();
        }

        private static string GetWithUnprocessedPaymentsSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Loan.Select(c => $"`ln`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Loan}` `ln` ");
            sql.Append("WHERE EXISTS (SELECT 1 ");
            sql.Append($"FROM {Constants.TableName.Payment} `pay` ");
            sql.Append("WHERE `pay`.`LoanId` = `ln`.`LoanId` ");
            sql.Append("AND `pay`.`Status` = 0 ");
            sql.AppendLine("LIMIT 1); ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanAgreement.Select(c => $"`lagmt`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.LoanAgreement}` `lagmt` ");
            sql.Append("WHERE EXISTS (SELECT 1 ");
            sql.Append($"FROM `{Constants.TableName.Payment}` `pay` ");
            sql.Append("WHERE `pay`.`LoanId` = `lagmt`.`LoanId` ");
            sql.Append("AND `pay`.`Status` = 0 ");
            sql.AppendLine("LIMIT 1); ");

            return sql.ToString();
        }

        private async Task<List<LoanData>> Read(DbDataReader reader)
        {
            List<LoanData> loans = (await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager))
                .ToList();
            GenericDataFactory<LoanAgreementData> agreementFactory = new GenericDataFactory<LoanAgreementData>();
            await reader.NextResultAsync();
            List<LoanAgreementData> agreements = (await agreementFactory.LoadData(reader, () => new LoanAgreementData(), DataUtil.AssignDataStateManager)).ToList();
            return loans.Join(
                agreements,
                ln => ln.LoanId,
                agr => agr.LoanId,
                (ln, agr) =>
                {
                    ln.Agreement = agr;
                    return ln;
                })
                .ToList();
        }
    }
}
