using BrassLoon.Interface.Authorization;

namespace JestersCreditUnion.Batch.PaymentIntakeCommitter
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;
        private readonly ITokenService _tokenService;

        public SettingsFactory(Settings settings, ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public LoanSettings CreateLoan()
            => new LoanSettings(_settings, _tokenService);
    }
}
