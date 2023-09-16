using JestersCreditUnion.CommonAPI;
using BlAccount = BrassLoon.Interface.Account;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace API
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly BlAccount.ITokenService _tokenService;

        public SettingsFactory(BlAccount.ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public AuthorizationSettings CreateAuthorization(Settings settings, string token)
            => new AuthorizationSettings(settings.AuthorizationApiBaseAddress, token);

        public AuthorizationSettings CreateAuthorization(Settings settings)
        {
            return new AuthorizationSettings(_tokenService,
                settings.AuthorizationApiBaseAddress,
                settings.BrassLoonAccountApiBaseAddress,
                settings.BrassLoonLogClientId,
                settings.BrassLoonLogClientSecret);
        }

        public ConfigurationSettings CreateConfiguration(Settings settings)
        {
            return new ConfigurationSettings(_tokenService,
                settings.BrassLoonConfigApiBaseAddress,
                settings.BrassLoonAccountApiBaseAddress,
                settings.BrassLoonLogClientId,
                settings.BrassLoonLogClientSecret);
        }

        public LogSettings CreateLog(Settings settings)
        {
            return new LogSettings(_tokenService,
                settings.BrassLoonLogApiBaseAddress,
                settings.BrassLoonAccountApiBaseAddress,
                settings.BrassLoonLogClientId,
                settings.BrassLoonLogClientSecret);
        }

        public WorkTaskSettings CreateWorkTask(Settings settings)
        {
            return new WorkTaskSettings(_tokenService,
                settings.BrassLoonWorkTaskApiBaseAddress,
                settings.BrassLoonAccountApiBaseAddress,
                settings.BrassLoonLogClientId,
                settings.BrassLoonLogClientSecret
                );
        }
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure