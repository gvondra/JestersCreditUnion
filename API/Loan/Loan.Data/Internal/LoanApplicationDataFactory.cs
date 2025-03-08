using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class LoanApplicationDataFactory : DataFactoryBase<LoanApplicationData>, ILoanApplicationDataFactory
    {
        private readonly IGenericDataFactory<LoanApplicationDenialData> _denialDataFactory;
        private readonly IGenericDataFactory<LoanApplicationCommentData> _commentDataFactory;
        private readonly IGenericDataFactory<IdentificationCardData> _identificationCardDataFactory;

        public LoanApplicationDataFactory(
            IDbProviderFactory providerFactory,
            IGenericDataFactory<LoanApplicationData> dataFactory,
            IGenericDataFactory<LoanApplicationDenialData> denialDataFactory,
            IGenericDataFactory<LoanApplicationCommentData> commentDataFactory,
            IGenericDataFactory<IdentificationCardData> identificationCardDataFactory)
            : base(providerFactory, dataFactory)
        {
            _denialDataFactory = denialDataFactory;
            _commentDataFactory = commentDataFactory;
            _identificationCardDataFactory = identificationCardDataFactory;
        }

        public async Task<LoanApplicationData> Get(ISettings settings, Guid id)
        {
            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(ProviderFactory, "id", DbType.Binary, DataUtil.GetParameterValueBinary(id))
            ];
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            IEnumerable<LoanApplicationData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<LoanApplicationData>> GetByUserId(ISettings settings, Guid userId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "userId", DbType.Binary, DataUtil.GetParameterValueBinary(userId))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            IEnumerable<LoanApplicationData> result = await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                GetByUserIdSql(),
                commandType: CommandType.Text,
                parameters: parameters,
                Read);
            return result.ToList();
        }

        private static string GetSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanApplication.Select(c => $"`lapp`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.LoanApplication} `lapp` ");
            sql.AppendLine("WHERE `lapp`.`LoanApplicationId` = @id ");
            sql.AppendLine("LIMIT 1; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanApplicationDenial.Select(c => $"`lad`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.LoanApplicationDenial} `lad` ");
            sql.AppendLine("WHERE `lad`.`LoanApplicationId` = @id ");
            sql.AppendLine("LIMIT 1; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanApplicationComment.Select(c => $"`cmt`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.LoanApplicationComment} `cmt` ");
            sql.AppendLine("WHERE `cmt`.`LoanApplicationId` = @loanApplicationId ");
            sql.AppendLine("ORDER BY `CreateTimestamp`; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.IdentificationCard.Select(c => $"`idc`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.IdentificationCard} `idc` ");
            sql.AppendLine($"INNER JOIN {Constants.TableName.LoanApplication} `lapp` on `idc`.`IdentificationCardId` = `lapp`.`BorrowerIdentificationCardId` ");
            sql.AppendLine("WHERE `lapp`.`LoanApplicationId` = @loanApplicationId; ");

            return sql.ToString();
        }

        private static string GetByUserIdSql()
        {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanApplication.Select(c => $"`lapp`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.LoanApplication} `lapp` ");
            sql.AppendLine("WHERE `lapp`.`UserId` = @userId ");
            sql.AppendLine("ORDER BY `lapp`.`CreateTimestamp` DESC; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanApplicationDenial.Select(c => $"`lad`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.LoanApplicationDenial} `lad` ");
            sql.Append("WHERE EXISTS (SELECT 1 ");
            sql.Append($"FROM {Constants.TableName.LoanApplication} `lapp` ");
            sql.Append("WHERE `lapp`.`LoanApplicationId` = `lad`.`LoanApplicationId` AND `lapp`.`UserId` = @userId ");
            sql.AppendLine("LIMIT 1); ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.LoanApplicationComment.Select(c => $"`cmt`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.LoanApplicationComment} `cmt` ");
            sql.Append($"WHERE EXISTS (SELECT 1 FROM {Constants.TableName.LoanApplication} `lapp` ");
            sql.Append("WHERE `lapp`.`LoanApplicationId` = `cmt`.`LoanApplicationId` AND `lapp`.`UserId` = @userId ");
            sql.AppendLine("LIMIT 1) ");
            sql.AppendLine("ORDER BY `CreateTimestamp`; ");

            sql.Append("SELECT ");
            sql.AppendLine(string.Join(", ", Constants.Column.IdentificationCard.Select(c => $"`idc`.`{c}`")));
            sql.AppendLine($"FROM {Constants.TableName.IdentificationCard} `idc` ");
            sql.AppendLine($"INNER JOIN {Constants.TableName.LoanApplication} `lapp` on `idc`.`IdentificationCardId` = `lapp`.`BorrowerIdentificationCardId` ");
            sql.AppendLine("WHERE `lapp`.`UserId` = @userId; ");

            return sql.ToString();
        }

        private async Task<IEnumerable<LoanApplicationData>> Read(DbDataReader reader)
        {
            IEnumerable<LoanApplicationData> result = await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager);
            IEnumerable<LoanApplicationDenialData> denials = Enumerable.Empty<LoanApplicationDenialData>();
            IEnumerable<LoanApplicationCommentData> comments = Enumerable.Empty<LoanApplicationCommentData>();
            IEnumerable<IdentificationCardData> identificationCards = Enumerable.Empty<IdentificationCardData>();
            if (await reader.NextResultAsync())
                denials = (await _denialDataFactory.LoadData(reader, () => new LoanApplicationDenialData(), DataUtil.AssignDataStateManager)).ToList();
            if (await reader.NextResultAsync())
                comments = (await _commentDataFactory.LoadData(reader, () => new LoanApplicationCommentData(), DataUtil.AssignDataStateManager)).ToList();
            if (await reader.NextResultAsync())
                identificationCards = (await _identificationCardDataFactory.LoadData(reader, () => new IdentificationCardData(), DataUtil.AssignDataStateManager)).ToList();

            result = result
                .GroupJoin(
                denials,
                a => a.LoanApplicationId,
                d => d.LoanApplicationId,
                (app, denials) =>
                {
                    app.Denial = denials.FirstOrDefault();
                    return app;
                })
                .GroupJoin(
                comments,
                a => a.LoanApplicationId,
                c => c.LoanApplicationId,
                (app, cmmnts) =>
                {
                    app.Comments = cmmnts.ToList();
                    return app;
                })
                .GroupJoin(
                identificationCards,
                a => a.BorrowerIdentificationCardId,
                c => c.IdentificationCardId,
                (app, crds) =>
                {
                    if (app.BorrowerIdentificationCardId.HasValue)
                    {
                        app.BorrowerIdentificationCard = crds.FirstOrDefault(c => c.IdentificationCardId.Equals(app.BorrowerIdentificationCardId.Value));
                    }
                    return app;
                });

            return result.ToList();
        }
    }
}
