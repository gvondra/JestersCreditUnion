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

        public DataSettings CreateDestinationData() => new DataSettings(_settings.DestinationConnectionString, _settings.UseDefaultAzureToken);

        public LoanApiSettings CreateLoanApiSettings() => new LoanApiSettings(_settings, _tokenService);

        public DataSettings CreateSourceData() => new DataSettings(_settings.SourceConnectionString, _settings.UseDefaultAzureToken);
    }
}
