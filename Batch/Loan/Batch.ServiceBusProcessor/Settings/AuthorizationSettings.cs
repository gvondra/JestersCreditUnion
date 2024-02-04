using BrassLoon.Interface.Authorization;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class AuthorizationSettings : ISettings
    {
        private readonly Settings _settings;

        public AuthorizationSettings(Settings settings)
        {
            _settings = settings;
        }

        public string BaseAddress => _settings.BrassLoonAuthorizationApiBaseAddress;

        public Task<string> GetToken()
        {
            throw new System.NotImplementedException();
        }
    }
}
