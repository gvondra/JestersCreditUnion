using JestersCreditUnion.Loan.Framework;
using AccountAPI = BrassLoon.Interface.Account;

namespace JestersCreditUnion.Loan.Core
{
    public class SettingsFactory
    {
        private readonly AccountAPI.ITokenService _tokenService;

        public SettingsFactory(AccountAPI.ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        internal ConfigurationSettings CreateConfiguration(ISettings settings)
            => new ConfigurationSettings(settings, _tokenService);

        internal WorkTaskSettings CreateWorkTask(ISettings settings)
            => new WorkTaskSettings(settings, _tokenService);

        internal AddressSettings CreateAddress(ISettings settings)
            => new AddressSettings(settings, _tokenService);
    }
}
