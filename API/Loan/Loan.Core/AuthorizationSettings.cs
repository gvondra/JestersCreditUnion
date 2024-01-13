using BrassLoon.Interface.Authorization;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    internal class AuthorizationSettings : ISettings
    {
        private readonly Framework.ISettings _settings;

        public AuthorizationSettings(Framework.ISettings settings)
        {
            _settings = settings;
        }

        public string BaseAddress => _settings.BrassLoonAuthorizationApiBaseAddress;

        public Task<string> GetToken() => throw new System.NotImplementedException();
    }
}
