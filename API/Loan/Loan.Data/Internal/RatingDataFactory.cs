using JestersCreditUnion.Loan.Data.Models;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class RatingDataFactory : DataFactoryBase<RatingData>, IRatingDataFactory
    {
        public RatingDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<RatingData> dataFactory)
            : base(providerFactory, dataFactory)
        { }

        public async Task<RatingData> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId)
        {
            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(ProviderFactory, "loanApplicationId", DbType.Guid, DataUtil.GetParameterValue(loanApplicationId))
            ];
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            RatingData data = null;
            await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                "[ln].[GetRating_by_LoanApplicationId]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) =>
                {
                    data = (await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).FirstOrDefault();
                    if (data != null)
                    {
                        GenericDataFactory<RatingLogData> ratingLogFactory = new GenericDataFactory<RatingLogData>();
                        await reader.NextResultAsync();
                        data.RatingLogs = (await ratingLogFactory.LoadData(reader, () => new RatingLogData(), DataUtil.AssignDataStateManager)).ToList();
                    }
                });
            return data;
        }
    }
}
