using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ISettings
    {
        string DatabaseHost { get; }
        string DatabaseName { get; }
        string DatabaseUser { get; }
        Task<string> GetDatabasePassword();
    }
}
