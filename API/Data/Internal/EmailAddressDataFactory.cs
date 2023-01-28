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
    public class EmailAddressDataFactory : DataFactoryBase<EmailAddressData>, IEmailAddressDataFactory
    {
        public EmailAddressDataFactory(ISqlDbProviderFactory providerFactory) : base(providerFactory) { }

        protected override EmailAddressData Create() => new EmailAddressData();

        public async Task<EmailAddressData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(this.ProviderFactory, "id", DbType.Guid, id)
            };
            return (await GenericDataFactory.GetData(
                settings,
                this.ProviderFactory,
                "[jcu].[GetEmailAddress]",
                this.Create,
                DataUtil.AssignDataStateManager,
                parameters
                )).FirstOrDefault();
        }

        public async Task<EmailAddressData> Get(ISqlSettings settings, string address)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(this.ProviderFactory, "address", DbType.AnsiString, address)
            };
            return (await GenericDataFactory.GetData(
                settings,
                this.ProviderFactory,
                "[jcu].[GetEmailAddress_by_Address]",
                this.Create,
                DataUtil.AssignDataStateManager,
                parameters
                )).FirstOrDefault();
        }
    }
}
