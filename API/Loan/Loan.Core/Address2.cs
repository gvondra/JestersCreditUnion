using BrassLoon.Interface.Address;
using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;
using Models = BrassLoon.Interface.Address.Models;

namespace JestersCreditUnion.Loan.Core
{
    // address 2 is based on data stored in BrassLoon
    public class Address2 : IAddress
    {
        private readonly IAddressService _addressService;
        private readonly SettingsFactory _settingsFactory;
        private Models.Address _innerAddress;

        public Address2(Models.Address innerAddress, IAddressService addressService, SettingsFactory settingsFactory)
        {
            _innerAddress = innerAddress;
            _addressService = addressService;
            _settingsFactory = settingsFactory;
        }

        public Guid AddressId => _innerAddress.AddressId.Value;

        public byte[] Hash => throw new NotImplementedException();

        public string Recipient => _innerAddress.Addressee;

        public string Attention => _innerAddress.Attention;

        public string Delivery => _innerAddress.Delivery;

        public string Secondary => _innerAddress.Secondary;

        public string City => _innerAddress.City;

        public string State => _innerAddress.Territory;

        public string PostalCode => _innerAddress.PostalCode;

        public DateTime CreateTimestamp => _innerAddress.CreateTimestamp ?? DateTime.UtcNow;

        private Guid? DomainId { get => _innerAddress.DomainId; set => _innerAddress.DomainId = value; }

        public async Task Create(ITransactionHandler transactionHandler, Framework.ISettings settings)
        {
            if (!DomainId.HasValue)
                DomainId = settings.AddressDomainId.Value;
            _innerAddress = await _addressService.Save(_settingsFactory.CreateAddress(settings), _innerAddress);
        }
    }
}
