using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class EmailAddressFactory : IEmailAddressFactory
    {
        private readonly IEmailAddressDataFactory _dataFactory;
        private readonly IEmailAddressDataSaver _dataSaver;

        public EmailAddressFactory(IEmailAddressDataFactory dataFactory, IEmailAddressDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
        }

        private EmailAddress Create(EmailAddressData data) => new EmailAddress(data, _dataSaver);

        public IEmailAddress Create(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));
            else if (!Regex.IsMatch(address, @".+@.+"))
                throw new ApplicationException("Invalid email address " + address);
            return Create(new EmailAddressData() { EmailAddressId = Guid.NewGuid(), Address = address });
        }

        public async Task<IEmailAddress> Get(ISettings settings, Guid id)
        {
            IEmailAddress result = null;
            EmailAddressData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            if (data != null)
                result = Create(data);
            return result;
        }

        public async Task<IEmailAddress> Get(ISettings settings, string address)
        {
            IEmailAddress result = null;
            EmailAddressData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), address);
            if (data != null)
                result = Create(data);
            return result;
        }
    }
}
