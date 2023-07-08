using JestersCreditUnion.Data.Models;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;

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
                async (DbDataReader reader) => data = (await Load(reader)).FirstOrDefault());
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
                async (DbDataReader reader) => data = (await Load(reader)).FirstOrDefault());
            return data;
        }

        public async Task<IEnumerable<LoanData>> GetByNameBirthDate(ISqlSettings settings, string name, DateTime birthDate)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            name = string.Format(
                CultureInfo.InvariantCulture,
                "%{0}%",
                name.Replace(@"\", @"\\").Replace(@"_", @"\_").Replace(@"%", @"\%"));
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "name", DbType.String, DataUtil.GetParameterValue(name)),
                DataUtil.CreateParameter(_providerFactory, "birthDate", DbType.Date, DataUtil.GetParameterValue(birthDate))
            };
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            List<LoanData> data = null;
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetLoan_by_BorrowerNameBirthDate]",
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
                async (DbDataReader reader) => data = (await Load(reader)).FirstOrDefault());
            return data;
        }

        private async Task<List<LoanData>> Load(DbDataReader reader)
        {
            List<LoanData> loans = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager))
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
