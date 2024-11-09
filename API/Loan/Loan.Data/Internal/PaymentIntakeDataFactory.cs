using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentIntakeDataFactory : DataFactoryBase<PaymentIntakeData>, IPaymentIntakeDataFactory
    {
        public PaymentIntakeDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory)
        { }

        public async Task<PaymentIntakeData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = [
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, id)
            ];
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetPaymentIntake]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }

        public Task<IEnumerable<PaymentIntakeData>> GetByStatuses(ISqlSettings settings, IEnumerable<short> statuses)
        {
            IDataParameter[] parameters = [
                DataUtil.CreateParameter(_providerFactory, "statues", DbType.AnsiString, string.Join(",", statuses))
            ];
            return _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetPaymentIntake_by_Statuses]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
