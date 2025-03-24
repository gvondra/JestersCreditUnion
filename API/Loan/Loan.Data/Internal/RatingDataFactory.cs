using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class RatingDataFactory : DataFactoryBase<RatingData>, IRatingDataFactory
    {
        private readonly IGenericDataFactory<RatingLogData> _ratingLogDataFactory;

        public RatingDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<RatingData> dataFactory, IGenericDataFactory<RatingLogData> ratingLogDataFactory)
            : base(providerFactory, dataFactory)
        {
            _ratingLogDataFactory = ratingLogDataFactory;
        }

        public async Task<RatingData> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId)
        {
            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(ProviderFactory, "loanApplicationId", DbType.Binary, DataUtil.GetParameterValueBinary(loanApplicationId))
            ];
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            IEnumerable<RatingData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByLoanApplicationIdSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
            return result?.FirstOrDefault();
        }

        private static string GetByLoanApplicationIdSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.Rating.Select(c => $"`rt`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.Rating}` `rt` ");
            sql.AppendLine($"INNER JOIN `{Constants.TableName.LoanApplicationRating}` `lar` ON `rt`.`RatingId` = `lar`.`RatingId` ");
            sql.AppendLine("WHERE `lar`.`LoanApplicationId` = @loanApplicationId; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.RatingLog.Select(c => $"`lg`.`{c}`")));
            sql.AppendLine($"FROM `{Constants.TableName.RatingLog}` `lg` ");
            sql.AppendLine($"INNER JOIN `{Constants.TableName.LoanApplicationRating}` `lar` ON `lg`.`RatingId` = `lar`.`RatingId` ");
            sql.AppendLine("WHERE `lar`.`LoanApplicationId` = @loanApplicationId; ");

            return sql.ToString();
        }

        private async Task<IEnumerable<RatingData>> Read(DbDataReader reader)
        {
            IEnumerable<RatingData> result = await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager);
            await reader.NextResultAsync();
            IEnumerable<RatingLogData> logData = await _ratingLogDataFactory.LoadData(reader, () => new RatingLogData(), DataUtil.AssignDataStateManager);
            return result
                .GroupJoin(
                logData,
                r => r.RatingId,
                lg => lg.RatingId,
                (r, lg) =>
                {
                    r.RatingLogs = lg.ToList();
                    return r;
                })
                .ToList();
        }
    }
}
