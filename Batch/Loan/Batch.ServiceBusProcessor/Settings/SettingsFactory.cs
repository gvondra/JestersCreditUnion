using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;
        private readonly AuthorizationAPI.ITokenService _tokenService;

        public SettingsFactory(Settings settings, AuthorizationAPI.ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public LoanSettings CreateLoan() => new LoanSettings(_settings, _tokenService);
    }
}
