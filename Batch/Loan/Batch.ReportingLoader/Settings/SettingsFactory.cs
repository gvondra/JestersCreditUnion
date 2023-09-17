using Api = JestersCreditUnion.Interface;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;
        private readonly Api.ITokenService _tokenService;

        public SettingsFactory(Settings settings, Api.ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public DataSettings CreateData() => new DataSettings(_settings);

        public LoanApiSettings CreateLoanApiSettings() => new LoanApiSettings(_settings, _tokenService);
    }
}
