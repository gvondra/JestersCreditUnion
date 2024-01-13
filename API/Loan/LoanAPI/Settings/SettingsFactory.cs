using BrassLoon.Interface.Account;
using JestersCreditUnion.CommonAPI;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace LoanAPI
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly IOptions<Settings> _options;
        private readonly ITokenService _tokenService;

        public SettingsFactory(IOptions<Settings> options, ITokenService tokenService)
        {
            _options = options;
            _tokenService = tokenService;
        }

        public AuthorizationSettings CreateAuthorization()
        {
            return new AuthorizationSettings(
                _tokenService,
                _options.Value.BrassLoonAuthorizationApiBaseAddress,
                _options.Value.BrassLoonAccountApiBaseAddress,
                _options.Value.BrassLoonClientId,
                _options.Value.BrassLoonClientSecret);
        }

        public ConfigurationSettings CreateConfiguration()
        {
            return new ConfigurationSettings(
                _tokenService,
                _options.Value.BrassLoonConfigApiBaseAddress,
                _options.Value.BrassLoonAccountApiBaseAddress,
                _options.Value.BrassLoonClientId,
                _options.Value.BrassLoonClientSecret);
        }

        public CoreSettings CreateCore() => new CoreSettings(_options.Value);
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
