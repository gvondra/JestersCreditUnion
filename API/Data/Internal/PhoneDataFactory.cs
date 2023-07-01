using JestersCreditUnion.Data.Models;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PhoneDataFactory : DataFactoryBase<PhoneData>, IPhoneDataFactory
    {
        public PhoneDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<PhoneData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, DataUtil.GetParameterValue(id))
            };
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetPhone]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }

        public async Task<PhoneData> Get(ISqlSettings settings, string number)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "number", DbType.AnsiString, DataUtil.GetParameterValue(number))
            };
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetPhone_by_Number]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }
    }
}
