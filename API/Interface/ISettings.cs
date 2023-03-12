using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ISettings
    {
        string BaseAddress { get; }

        Task<string> GetToken();
    }
}
