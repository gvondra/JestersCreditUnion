using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class AddressDataFactory : DataFactoryBase<AddressData>, IAddressDataFactory
    {
        public AddressDataFactory(ISqlDbProviderFactory providerFactory) : base(providerFactory) { }

        protected override AddressData Create() => new AddressData();

        public async Task<AddressData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(this.ProviderFactory, "id", DbType.Guid, id)
            };
            return (await GenericDataFactory.GetData(
                settings,
                this.ProviderFactory,
                "[jcu].[GetAddress]",
                this.Create,
                DataUtil.AssignDataStateManager,
                parameters
                )).FirstOrDefault();
        }

        public Task<IEnumerable<AddressData>> GetByHash(ISqlSettings settings, byte[] hash)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(this.ProviderFactory, "hash", DbType.Binary, hash)
            };
            return GenericDataFactory.GetData(
                settings,
                this.ProviderFactory,
                "[jcu].[GetAddress_by_Hash]",
                this.Create,
                DataUtil.AssignDataStateManager,
                parameters
                );
        }
    }
}
