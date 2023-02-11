using JestersCreditUnion.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class DataSettings : JestersCreditUnion.Data.IDataSettings
    {
        private readonly ISettings _settings;

        public DataSettings(ISettings settings) 
        {
            _settings = settings;
        }

        public string Host => _settings.DatabaseHost;

        public string DatabaseName => _settings.DatabaseName;

        public string DatabaseUser => _settings.DatabaseUser;

        public Task<string> GetDatabasePassword() => _settings.GetDatabasePassword();
    }
}
