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
            List<IdentificationCardData> identificationCards = null;
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
                        GenericDataFactory<IdentificationCardData> identificationCardFactory = new GenericDataFactory<IdentificationCardData>();
                        await reader.NextResultAsync();
                        data.Denial = (await denialFactory.LoadData(reader, () => new LoanApplicationDenialData(), DataUtil.AssignDataStateManager)).FirstOrDefault();
                        await reader.NextResultAsync();
                        data.Comments = (await commentFactory.LoadData(reader, () => new LoanApplicationCommentData(), DataUtil.AssignDataStateManager)).ToList();
                        await reader.NextResultAsync();
                        identificationCards = (await identificationCardFactory.LoadData(reader, () => new IdentificationCardData(), DataUtil.AssignDataStateManager)).ToList();
                    }
                });
            if (data.BorrowerIdentificationCardId.HasValue && identificationCards != null)
            {
                data.BorrowerIdentificationCard = identificationCards.FirstOrDefault(c => c.IdentificationCardId.Equals(data.BorrowerIdentificationCardId.Value));
            }
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
            List<IdentificationCardData> identificationCards = null;
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
                    GenericDataFactory<IdentificationCardData> identificationCardFactory = new GenericDataFactory<IdentificationCardData>();
                    await reader.NextResultAsync();
                    denials = (await denialFactory.LoadData(reader, () => new LoanApplicationDenialData(), DataUtil.AssignDataStateManager)).ToList();
                    await reader.NextResultAsync();
                    comments = (await commentFactory.LoadData(reader, () => new LoanApplicationCommentData(), DataUtil.AssignDataStateManager)).ToList();
                    await reader.NextResultAsync();
                    identificationCards = (await identificationCardFactory.LoadData(reader, () => new IdentificationCardData(), DataUtil.AssignDataStateManager)).ToList();
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
