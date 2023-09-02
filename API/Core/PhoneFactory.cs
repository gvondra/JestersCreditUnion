using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PhoneFactory : IPhoneFactory
    {
        private readonly IPhoneDataFactory _dataFactory;
        private readonly IPhoneDataSaver _dataSaver;

        public PhoneFactory(IPhoneDataFactory dataFactory, IPhoneDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
        }

        private Phone Create(PhoneData data) => new Phone(data, _dataSaver);

        public IPhone Create(ref string number)
        {
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException(nameof(number));
            number = Regex.Replace(number, @"[^0-9]+", string.Empty, RegexOptions.IgnoreCase);
            if (!Regex.IsMatch(number, @"^[0-9]{10}$", RegexOptions.IgnoreCase))
                throw new ApplicationException("Invalid phone number " + number);
            return Create(new PhoneData() { PhoneId = Guid.NewGuid(), Number = number });
        }

        public async Task<IPhone> Get(ISettings settings, Guid id)
        {
            IPhone result = null;
            PhoneData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            if (data != null)
                result = Create(data);
            return result;
        }

        public async Task<IPhone> Get(ISettings settings, string number)
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));
            number = Regex.Replace(number, @"[^0-9]+", string.Empty, RegexOptions.IgnoreCase);
            IPhone result = null;
            PhoneData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), number);
            if (data != null)
                result = Create(data);
            return result;
        }
    }
}
