using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PaymentDataFactory : DataFactoryBase<PaymentData>, IPaymentDataFactory
    {
        public PaymentDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISqlSettings settings, DateTime date, string loanNumber, string transactionNumber)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "date", DbType.Date, DataUtil.GetParameterValue(date)),
                DataUtil.CreateParameter(_providerFactory, "loanNumber", DbType.AnsiString, DataUtil.GetParameterValue(loanNumber)),
                DataUtil.CreateParameter(_providerFactory, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(transactionNumber))
            };
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetPayment_by_Date_LoanNumber_TransactionNumber]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }

        public Task<IEnumerable<PaymentData>> GetByStatus(ISqlSettings settings, short status)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "status", DbType.Int16, DataUtil.GetParameterValue(status))
            };
            return _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetPayment_by_Status]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
