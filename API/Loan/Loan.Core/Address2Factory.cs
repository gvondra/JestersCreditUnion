using BrassLoon.Interface.Address;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models = BrassLoon.Interface.Address.Models;

namespace JestersCreditUnion.Loan.Core
{
    public class Address2Factory : IAddressFactory
    {
        private readonly IAddressService _addressService;
        private readonly SettingsFactory _settingsFactory;

        public Address2Factory(IAddressService addressService, SettingsFactory settingsFactory)
        {
            _addressService = addressService;
            _settingsFactory = settingsFactory;
        }

        private Address2 Create(Models.Address data) => new Address2(data, _addressService, _settingsFactory);

        public IAddress Create(string recipient, string attention, string delivery, string secondary, string city, ref string state, ref string postalCode)
        {
            if (!string.IsNullOrEmpty(state))
            {
                state = state.Trim().ToUpper(CultureInfo.InvariantCulture);
                if (!Regex.IsMatch(state, @"^[A-Z]{2}$", RegexOptions.None, TimeSpan.FromMilliseconds(200)))
                    throw new ApplicationException($"Invalid state value \"{state}\"");
            }
            if (!string.IsNullOrEmpty(postalCode))
            {
                postalCode = Regex.Replace(postalCode.Trim(), @"^[^0-9]+$", string.Empty, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));
                if (!Regex.IsMatch(postalCode, @"^[0-9]{5}([0-9]{4})?$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200)))
                    throw new ApplicationException($"Invalid postal code \"{postalCode}\"");
            }
            return Create(new Models.Address
            {
                Addressee = recipient ?? string.Empty,
                Attention = attention ?? string.Empty,
                Delivery = delivery ?? string.Empty,
                Secondary = secondary ?? string.Empty,
                City = city ?? string.Empty,
                Territory = state ?? string.Empty,
                PostalCode = postalCode ?? string.Empty
            });
        }

        public IAddress Create(string recipient, string delivery, string city, ref string state, ref string postalCode)
            => Create(recipient, string.Empty, delivery, string.Empty, city, ref state, ref postalCode);

        public async Task<IAddress> Get(Framework.ISettings settings, Guid id)
        {
            Models.Address data = await _addressService.Get(_settingsFactory.CreateAddress(settings), settings.AddressDomainId.Value, id);
            Address2 result = data != null ? Create(data) : null;
            return result;
        }

        public Task<IAddress> GetByHash(Framework.ISettings settings, byte[] hash) => throw new NotImplementedException();
    }
}
