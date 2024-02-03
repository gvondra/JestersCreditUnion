using JestersCreditUnion.Interface;
using System.Threading.Tasks;
using AuthorizatinAPI = BrassLoon.Interface.Authorization;

namespace JestersCreditUnion.Loan.Core
{
    internal sealed class ApiSettings : ISettings
    {
        private readonly Framework.ISettings _settings;
        private readonly AuthorizatinAPI.ITokenService _tokenService;

        public ApiSettings(Framework.ISettings settings, AuthorizatinAPI.ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public string BaseAddress => _settings.ApiBaseAddress;

        public Task<string> GetToken()
            => _tokenService.CreateClientCredential(new AuthorizationSettings(_settings), _settings.AuthorizationDomainId.Value, _settings.ClientId.Value, _settings.ClientSecret);
    }
}
