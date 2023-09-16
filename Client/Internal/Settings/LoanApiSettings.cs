using JestersCreditUnion.Interface.Loan;
using System.Threading.Tasks;

namespace JCU.Internal
{
    public class LoanApiSettings : ISettings
    {
        private readonly string _baseAddress;
        private readonly string _token;

        public LoanApiSettings(string baseAddress, string token)
        {
            _baseAddress = baseAddress;
            _token = token;
        }

        public string BaseAddress => _baseAddress;

        public Task<string> GetToken() => Task.FromResult(_token);
    }
}
