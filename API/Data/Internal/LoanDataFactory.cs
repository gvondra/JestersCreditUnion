using JestersCreditUnion.Data.Models;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanDataFactory : DataFactoryBase<LoanData>, ILoanDataFactory
    {
        public LoanDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<LoanData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, DataUtil.GetParameterValue(id))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            LoanData data = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetLoan]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) => data = await Load(reader));
            return data;
        }

        public async Task<LoanData> GetByLoanApplicationId(ISqlSettings settings, Guid loanApplicationId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "loanApplicationId", DbType.Guid, DataUtil.GetParameterValue(loanApplicationId))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            LoanData data = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetLoan_by_LoanApplicationId]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) => data = await Load(reader));
            return data;
        }

        public async Task<LoanData> GetByNumber(ISqlSettings settings, string number)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "number", DbType.AnsiString, DataUtil.GetParameterValue(number))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            LoanData data = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetLoan_by_Number]",
                commandType: CommandType.StoredProcedure,
                parameters: parameters,
                async (DbDataReader reader) => data = await Load(reader));
            return data;
        }

        private async Task<LoanData> Load(DbDataReader reader)
        {
            LoanData data = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).FirstOrDefault();
            if (data != null)
            {
                GenericDataFactory<LoanAgreementData> agreementFactory = new GenericDataFactory<LoanAgreementData>();
                reader.NextResult();
                data.Agreement = (await agreementFactory.LoadData(reader, () => new LoanAgreementData(), DataUtil.AssignDataStateManager)).FirstOrDefault();
            }
            return data;
        }
    }
}
