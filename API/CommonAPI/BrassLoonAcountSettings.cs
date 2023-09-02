using BrassLoon.Interface.Account;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonAPI
{
    public class BrassLoonAcountSettings : ISettings
    {
        public string BaseAddress { get; set; }

        public Task<string> GetToken() => throw new NotImplementedException();
    }
}
