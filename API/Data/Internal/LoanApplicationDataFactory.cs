using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanApplicationDataFactory : DataFactoryBase<LoanApplicationData>, ILoanApplicationDataFactory
    {
        public LoanApplicationDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<LoanApplicationData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, DataUtil.GetParameterValue(id))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            LoanApplicationData data = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetLoanApplication]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) =>
                {
                    data = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).FirstOrDefault();
                    if (data != null)
                    {
                        GenericDataFactory<LoanApplicationDenialData> denialFactory = new GenericDataFactory<LoanApplicationDenialData>();
                        GenericDataFactory<LoanApplicationCommentData> commentFactory = new GenericDataFactory<LoanApplicationCommentData>();
                        reader.NextResult();
                        data.Denial = (await denialFactory.LoadData(reader, () => new LoanApplicationDenialData(), DataUtil.AssignDataStateManager)).FirstOrDefault();
                        reader.NextResult();
                        data.Comments = (await commentFactory.LoadData(reader, () => new LoanApplicationCommentData(), DataUtil.AssignDataStateManager)).ToList();
                    }
                });
            return data;
        }

        public async Task<IEnumerable<LoanApplicationData>> GetByUserId(ISqlSettings settings, Guid userId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "userId", DbType.Guid, DataUtil.GetParameterValue(userId))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            IEnumerable<LoanApplicationData> result = null;
            List<LoanApplicationDenialData> denials = null;
            List<LoanApplicationCommentData> comments = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetLoanApplication_by_UserId]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) =>
                {
                    result = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).ToList();
                    GenericDataFactory<LoanApplicationDenialData> denialFactory = new GenericDataFactory<LoanApplicationDenialData>();
                    GenericDataFactory<LoanApplicationCommentData> commentFactory = new GenericDataFactory<LoanApplicationCommentData>();
                    reader.NextResult();
                    denials = (await denialFactory.LoadData(reader, () => new LoanApplicationDenialData(), DataUtil.AssignDataStateManager)).ToList();
                    reader.NextResult();
                    comments = (await commentFactory.LoadData(reader, () => new LoanApplicationCommentData(), DataUtil.AssignDataStateManager)).ToList();
                });
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
                });
            return result.ToList();
        }
    }
}
