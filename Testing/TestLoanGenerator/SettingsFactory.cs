using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;
        private readonly ITokenService _tokenService;
        private string _accessToken;

        public SettingsFactory(Settings settings, ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public async Task<ApiSettings> GetApiSettings()
        {
            return new ApiSettings
            {
                BaseAddress = _settings.ApiBaseAddress,
                AccessToken = await GetAccessToken()
            };
        }

        public async Task<LoanApiSettings> GetLoanApiSettings()
        {
            return new LoanApiSettings
            {
                BaseAddress = _settings.LoanApiBaseAddress,
                AccessToken = await GetAccessToken()
            };
        }

        private async Task<string> GetAccessToken()
        {   
            if (string.IsNullOrEmpty(_accessToken))
            {
                if (!_settings.ClientId.HasValue)
                    throw new ApplicationException("Client id and client secret must configured");
                _accessToken = await _tokenService.CreateClientCredential(
                    new ApiSettings { BaseAddress = _settings.ApiBaseAddress },
                    new ClientCredential { ClientId = _settings.ClientId.Value, Secret = _settings.ClientSecret });
            }
            return _accessToken;
        }
    }
}
