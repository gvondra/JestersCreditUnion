using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonCore
{
    public interface ISettings
    {
        public bool UseDefaultAzureSqlToken { get; }
        Task<string> GetConnetionString();
        Func<Task<string>> GetDatabaseAccessToken();
    }
}
