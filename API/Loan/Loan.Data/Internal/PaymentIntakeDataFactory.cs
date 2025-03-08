using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentIntakeDataFactory : DataFactoryBase<PaymentIntakeData>, IPaymentIntakeDataFactory
    {
        public PaymentIntakeDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<PaymentIntakeData> dataFactory)
            : base(providerFactory, dataFactory)
        { }

        public async Task<PaymentIntakeData> Get(ISettings settings, Guid id)
        {
            IDataParameter[] parameters = [
                DataUtil.CreateParameter(ProviderFactory, "id", DbType.Guid, id)
            ];
            return (await DataFactory.GetData(
                settings,
                ProviderFactory,
                "[ln].[GetPaymentIntake]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }

        public Task<IEnumerable<PaymentIntakeData>> GetByStatuses(ISettings settings, IEnumerable<short> statuses)
        {
            IDataParameter[] parameters = [
                DataUtil.CreateParameter(ProviderFactory, "statues", DbType.AnsiString, string.Join(",", statuses))
            ];
            return DataFactory.GetData(
                settings,
                ProviderFactory,
                "[ln].[GetPaymentIntake_by_Statuses]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
