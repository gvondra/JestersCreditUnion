using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Core
{
    public class AddressFactory : IAddressFactory
    {
        private readonly IAddressDataFactory _dataFactory;
        private readonly IAddressDataSaver _dataSaver;

        public AddressFactory(IAddressDataFactory dataFactory, IAddressDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
        }

        private Address Create(AddressData data) => new Address(data, _dataSaver);

        public IAddress Create(string recipient, string attention, string delivery, string secondary, string city, ref string state, ref string postalCode)
        {
            if (!string.IsNullOrEmpty(state))
            {
                state = state.Trim().ToUpper();
                if (!Regex.IsMatch(state, @"^[A-Z]{2}$"))
                    throw new ApplicationException($"Invalid state value \"{state}\"");
            }
            if (!string.IsNullOrEmpty(postalCode))
            {
                postalCode = Regex.Replace(postalCode.Trim(), @"^[^0-9]+$", string.Empty, RegexOptions.IgnoreCase);
                if (!Regex.IsMatch(postalCode, @"^[0-9]{5}([0-9]{4})?$", RegexOptions.IgnoreCase))
                    throw new ApplicationException($"Invalid postal code \"{postalCode}\"");
            }
            return Create(new AddressData
            {
                AddressId = Guid.NewGuid(),
                Recipient = recipient ?? string.Empty,
                Attention = attention ?? string.Empty,
                Delivery = delivery ?? string.Empty,
                Secondary = secondary ?? string.Empty,
                City = city ?? string.Empty,
                State = state ?? string.Empty,
                PostalCode = postalCode ?? string.Empty,
                Hash = ComputeHash(recipient, attention, delivery, secondary, city, state, postalCode)
            });
        }

        private byte[] ComputeHash(in string recipient, in string attention, in string delivery, in string secondary, in string city, in string state, in string postalCode)
        {
            string[] values = new string[]
            {
                (recipient ?? string.Empty).ToLower(),
                (attention ?? string.Empty).ToLower(),
                (delivery ?? string.Empty).ToLower(),
                (secondary ?? string.Empty).ToLower(),
                (city ?? string.Empty).ToLower(),
                (state ?? string.Empty).ToUpper(),
                (postalCode ?? string.Empty).ToLower()
            };
            string json = JsonConvert.SerializeObject(values, CreateSerializerSettings());
            using (SHA512 sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(Encoding.UTF8.GetBytes(json));
            }
        }

        private JsonSerializerSettings CreateSerializerSettings() => new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            ContractResolver = new DefaultContractResolver()
        };

        public IAddress Create(string recipient, string delivery, string city, ref string state, ref string postalCode)
            => Create(recipient, string.Empty, delivery, string.Empty, city, ref state, ref postalCode);

        public async Task<IAddress> Get(ISettings settings, Guid id)
        {
            IAddress result = null;
            AddressData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            if (data != null)
                result = Create(data);
            return result;
        }

        public async Task<IAddress> GetByHash(ISettings settings, byte[] hash)
        {
            IAddress result = null;
            IEnumerable<AddressData> data = await _dataFactory.GetByHash(new CommonCore.DataSettings(settings), hash);
            if (data != null && data.Count() > 0)
                result = Create(data.First());
            return result;
        }
    }
}
