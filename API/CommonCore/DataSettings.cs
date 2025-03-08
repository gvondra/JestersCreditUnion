using System.Threading.Tasks;

namespace JestersCreditUnion.CommonCore
{
    public class DataSettings : BrassLoon.DataClient.ISettings
    {
        private readonly ISettings _settings;

        public DataSettings(ISettings settings)
        {
            _settings = settings;
        }

        public Task<string> GetConnectionString() => _settings.GetConnetionString();
    }
}
