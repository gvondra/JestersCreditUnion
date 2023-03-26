using BrassLoon.Interface.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountAPI = BrassLoon.Interface.Account;

namespace JestersCreditUnion.CommonAPI
{
    public class ConfigurationSettings : ISettings
    {
        private readonly string _baseAddress;
        private readonly string _brassLoonAccountBaseAddress;
        private readonly Guid? _clientId;
        private readonly string _clientSecret;
        private readonly AccountAPI.ITokenService _tokenService;

        public ConfigurationSettings(AccountAPI.ITokenService tokenService,
            string baseAddress,
            string brassLoonAccountBaseAddress,
            Guid? clientId,
            string clientSecret)
        {
            _tokenService = tokenService;
            _baseAddress = baseAddress;
            _brassLoonAccountBaseAddress = brassLoonAccountBaseAddress;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public string BaseAddress => _baseAddress;

        public async Task<string> GetToken()
        {
            if (string.IsNullOrEmpty(_brassLoonAccountBaseAddress))
                throw new ArgumentException("Brass Loon Account base address property value not set");
            if (!_clientId.HasValue || _clientId.Value.Equals(Guid.Empty))
                throw new ArgumentException("Brass Loon Account client id property value not set");
            if (string.IsNullOrEmpty(_clientSecret))
                throw new ArgumentException("Brass Loon Account client secret property value not set");
            BrassLoonAcountSettings settings = new BrassLoonAcountSettings
            {
                BaseAddress = _brassLoonAccountBaseAddress
            };
            return await _tokenService.CreateClientCredentialToken(settings, _clientId.Value, _clientSecret);
        }
    }
}
