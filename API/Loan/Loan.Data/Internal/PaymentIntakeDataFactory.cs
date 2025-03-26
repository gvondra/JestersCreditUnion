using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentIntakeDataFactory : DataFactoryBase<PaymentIntakeData>, IPaymentIntakeDataFactory
    {
        public PaymentIntakeDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<PaymentIntakeData> dataFactory)
            : base(providerFactory, dataFactory)
        { }

        public async Task<PaymentIntakeData> Get(ISettings settings, Guid id)
        {
            IDataParameter[] parameters = [
                DataUtil.CreateParameter(ProviderFactory, "id", DbType.Binary, DataUtil.GetParameterValueBinary(id))
            ];
            return (await DataFactory.GetData(
                settings,
                ProviderFactory,
                GetSql(),
                Create,
                DataUtil.AssignDataStateManager,
                parameters,
                CommandType.Text))
                .FirstOrDefault();
        }

        public Task<IEnumerable<PaymentIntakeData>> GetByStatuses(ISettings settings, IEnumerable<short> statuses)
        {
            IDataParameter[] parameters = [
                DataUtil.CreateParameter(ProviderFactory, "statues", DbType.AnsiString, $"[{string.Join(",", statuses)}]")
            ];
            return DataFactory.GetData(
                settings,
                ProviderFactory,
                GetByStatusesSql(),
                Create,
                DataUtil.AssignDataStateManager,
                parameters,
                CommandType.Text);
        }

        private static string GetSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine($"FROM {Constants.TableName.PaymentIntake} `pIn` ");
            sql.AppendLine("WHERE `pIn`.`PaymentIntakeId` = @id; ");
            return sql.ToString();
        }

        private static string GetByStatusesSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.PaymentIntake.Select(c => $"`pIn`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.PaymentIntake} `pIn` ");
            sql.AppendLine("WHERE `pIn`.`Status` MEMBER OF(@statues) ");
            return sql.ToString();
        }
    }
}
