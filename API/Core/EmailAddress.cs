using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class EmailAddress : IEmailAddress
    {
        private readonly EmailAddressData _data;
        private readonly IEmailAddressDataSaver _dataSaver;

        public EmailAddress(EmailAddressData data, IEmailAddressDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid EmailAddressId => _data.EmailAddressId;

        public string Address => _data.Address ?? string.Empty;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public Task Create(ITransactionHandler transactionHandler) => _dataSaver.Create(transactionHandler, _data);
    }
}
