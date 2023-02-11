using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IDataSettings
    {
        public string Host { get; }
        public string DatabaseName { get; }
        public string DatabaseUser { get; }

        public Task<string> GetDatabasePassword();
    }
}
