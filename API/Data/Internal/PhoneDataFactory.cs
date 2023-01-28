using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PhoneDataFactory : DataFactoryBase<PhoneData>, IPhoneDataFactory
    {
        public PhoneDataFactory(ISqlDbProviderFactory providerFactory) : base(providerFactory) { }

        protected override PhoneData Create() => new PhoneData();

        public async Task<PhoneData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(this.ProviderFactory, "id", DbType.Guid, id)
            };
            return (await GenericDataFactory.GetData(
                settings,
                this.ProviderFactory,
                "[jcu].[GetPhone]",
                this.Create,
                DataUtil.AssignDataStateManager,
                parameters
                )).FirstOrDefault();
        }

        public async Task<PhoneData> Get(ISqlSettings settings, string number)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(this.ProviderFactory, "number", DbType.AnsiString, number)
            };
            return (await GenericDataFactory.GetData(
                settings,
                this.ProviderFactory,
                "[jcu].[GetPhone_by_Number]",
                this.Create,
                DataUtil.AssignDataStateManager,
                parameters
                )).FirstOrDefault();
        }
    }
}
