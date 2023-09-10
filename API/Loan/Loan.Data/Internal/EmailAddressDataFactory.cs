using JestersCreditUnion.Data.Models;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class EmailAddressDataFactory : DataFactoryBase<EmailAddressData>, IEmailAddressDataFactory
    {
        public EmailAddressDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<EmailAddressData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, DataUtil.GetParameterValue(id))
            };
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetEmailAddress]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }

        public async Task<EmailAddressData> Get(ISqlSettings settings, string address)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "address", DbType.AnsiString, DataUtil.GetParameterValue(address))
            };
            return (await _genericDataFactory.GetData(
                settings,
                _providerFactory,
                "[ln].[GetEmailAddress_by_Address]",
                Create,
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }
    }
}
