using BrassLoon.Interface.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonAPI
{
    public class BrassLoonAcountSettings : ISettings
    {
        public string BaseAddress { get; set; }

        public Task<string> GetToken()
        {
            throw new NotImplementedException();
        }
    }
}
