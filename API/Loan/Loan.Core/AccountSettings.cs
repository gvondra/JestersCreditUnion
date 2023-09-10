using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;
using AccountAPI = BrassLoon.Interface.Account;

namespace JestersCreditUnion.Loan.Core
{
    public class AccountSettings : AccountAPI.ISettings
    {
        private readonly ISettings _settings;

        public AccountSettings(ISettings settings)
        {
            _settings = settings;
        }

        public string BaseAddress => _settings.BrassLoonAccountApiBaseAddress;

        public Task<string> GetToken()
        {
            throw new NotImplementedException();
        }
    }
}
