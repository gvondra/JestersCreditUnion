using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class AddressDataFactory : DataFactoryBase<AddressData>, IAddressDataFactory
    {
        public AddressDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<AddressData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, DataUtil.GetParameterValue(id))
            };
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetAddress]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }

        public Task<IEnumerable<AddressData>> GetByHash(ISqlSettings settings, byte[] hash)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "hash", DbType.Binary, DataUtil.GetParameterValue(hash))
            };
            return _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetAddress_By_Hash]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
