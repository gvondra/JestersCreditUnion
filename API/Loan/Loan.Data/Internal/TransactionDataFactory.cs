using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class TransactionDataFactory : DataFactoryBase<TransactionData>, ITransactionDataFactory
    {
        public TransactionDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory)
        { }

        public Task<IEnumerable<TransactionData>> GetByLoanId(ISqlSettings settings, Guid loanId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "loanId", DbType.Guid, DataUtil.GetParameterValue(loanId))
            };
            return _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetTransaction_by_LoanId]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
