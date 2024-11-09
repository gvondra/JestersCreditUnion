using JestersCreditUnion.Loan.Framework;
using AccountAPI = BrassLoon.Interface.Account;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace JestersCreditUnion.Loan.Core
{
    public class SettingsFactory
    {
        private readonly AccountAPI.ITokenService _accountTokenService;
        private readonly AuthorizationAPI.ITokenService _authorizationTokenService;

        public SettingsFactory(AccountAPI.ITokenService tokenService, AuthorizationAPI.ITokenService authorizationTokenService)
        {
            _accountTokenService = tokenService;
            _authorizationTokenService = authorizationTokenService;
        }

        internal ConfigurationSettings CreateConfiguration(ISettings settings)
            => new ConfigurationSettings(settings, _accountTokenService);

        internal WorkTaskSettings CreateWorkTask(ISettings settings)
            => new WorkTaskSettings(settings, _accountTokenService);

        internal AddressSettings CreateAddress(ISettings settings)
            => new AddressSettings(settings, _accountTokenService);

        internal ApiSettings CreateApi(ISettings settings)
            => new ApiSettings(settings, _authorizationTokenService);
    }
}
