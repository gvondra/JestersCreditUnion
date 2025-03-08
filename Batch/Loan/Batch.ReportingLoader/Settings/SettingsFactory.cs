using Account = BrassLoon.Interface.Account;
using Api = JestersCreditUnion.Interface;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;
        private readonly Api.ITokenService _tokenService;
        private readonly Account.ITokenService _accountTokenService;

        public SettingsFactory(Settings settings, Api.ITokenService tokenService, Account.ITokenService accountTokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
            _accountTokenService = accountTokenService;
        }

        public AuthorizationSettings CreateAuthorizationSettings()
            => new AuthorizationSettings(_settings, _accountTokenService);

        public DataSettings CreateDestinationData() => new DataSettings(_settings.DestinationConnectionString);

        public LoanApiSettings CreateLoanApiSettings() => new LoanApiSettings(_settings, _tokenService);

        public DataSettings CreateSourceData() => new DataSettings(_settings.SourceConnectionString);
        
        public WorkTaskSettings CreateWorkTaskSettings() => new WorkTaskSettings(_settings, _accountTokenService);
    }
}
