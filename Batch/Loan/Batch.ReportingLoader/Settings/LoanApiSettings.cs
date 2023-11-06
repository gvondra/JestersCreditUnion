using JestersCreditUnion.Interface.Loan;
using System.Threading.Tasks;
using Api = JestersCreditUnion.Interface;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class LoanApiSettings : ISettings
    {
        private readonly Settings _settings;
        private readonly Api.ITokenService _tokenService;

        public LoanApiSettings(Settings settings, Api.ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public string BaseAddress => _settings.LoanApiBaseAddress;

        public Task<string> GetToken()
        {
            Api.Models.ClientCredential credential = new Api.Models.ClientCredential { ClientId = _settings.ClientId, Secret = _settings.ClientSecret };
            return _tokenService.CreateClientCredential(new ApiSettings(_settings), credential);
        }
    }
}
