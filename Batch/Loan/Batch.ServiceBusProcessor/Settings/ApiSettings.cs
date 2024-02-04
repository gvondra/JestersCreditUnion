using JestersCreditUnion.Interface;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class ApiSettings : ISettings
    {
        private readonly Settings _settings;
        private readonly AuthorizationAPI.ITokenService _tokenService;

        public ApiSettings(Settings settings, AuthorizationAPI.ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public string BaseAddress => _settings.ApiBaseAddress;

        public Task<string> GetToken()
            => _tokenService.CreateClientCredential(new AuthorizationSettings(_settings), _settings.AuthorizationDomainId.Value, _settings.ClientId.Value, _settings.ClientSecret);
    }
}
