using JestersCreditUnion.Interface;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class ApiSettings : ISettings
    {
        public string BaseAddress { get; set; }
        public string AccessToken { get; set; }

        public Task<string> GetToken() => Task.FromResult(AccessToken);
    }
}
