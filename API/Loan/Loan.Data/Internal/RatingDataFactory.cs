using JestersCreditUnion.Loan.Data.Models;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class RatingDataFactory : DataFactoryBase<RatingData>, IRatingDataFactory
    {
        public RatingDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory)
        { }

        public async Task<RatingData> GetByLoanApplicationId(ISqlSettings settings, Guid loanApplicationId)
        {

            IDataParameter[] parameters =
            [
                DataUtil.CreateParameter(_providerFactory, "loanApplicationId", DbType.Guid, DataUtil.GetParameterValue(loanApplicationId))
            ];
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            RatingData data = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetRating_by_LoanApplicationId]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) =>
                {
                    data = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).FirstOrDefault();
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
