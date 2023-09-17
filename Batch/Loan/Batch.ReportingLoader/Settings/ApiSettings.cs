using JestersCreditUnion.Interface;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class ApiSettings : ISettings
    {
        private readonly Settings _settings;

        public ApiSettings(Settings settings)
        {
            _settings = settings;
        }

        public string BaseAddress => _settings.ApiBaseAddress;

        public Task<string> GetToken()
        {
            // using client credential, so leaving as not implemented
            throw new System.NotImplementedException();
        }
    }
}
