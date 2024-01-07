using BrassLoon.Interface.Account;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    internal class AddressSettings : BrassLoon.Interface.Address.ISettings
    {
        private readonly ITokenService _tokenService;
        private readonly Framework.ISettings _settings;

        public AddressSettings(
            Framework.ISettings settings,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _settings = settings;
        }

        public string BaseAddress => _settings.BrassLoonAddressApiBaseAddress;

        public Task<string> GetToken()
            => _tokenService.CreateClientCredentialToken(new AccountSettings(_settings), _settings.BrassLoonClientId.Value, _settings.BrassLoonClientSecret);
    }
}
