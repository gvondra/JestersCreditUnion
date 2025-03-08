using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class TransactionDataFactory : DataFactoryBase<TransactionData>, ITransactionDataFactory
    {
        public TransactionDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<TransactionData> dataFactory)
            : base(providerFactory, dataFactory)
        { }

        public Task<IEnumerable<TransactionData>> GetByLoanId(ISettings settings, Guid loanId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "loanId", DbType.Guid, DataUtil.GetParameterValue(loanId))
            };
            return DataFactory.GetData(
                settings,
                ProviderFactory,
                "[ln].[GetTransaction_by_LoanId]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
