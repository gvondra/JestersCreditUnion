using System.Threading.Tasks;

namespace JestersCreditUnion.CommonCore
{
    public interface ISettings
    {
        Task<string> GetConnetionString();
    }
}
