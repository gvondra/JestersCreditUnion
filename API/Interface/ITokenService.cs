using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ITokenService
    {
        Task<string> Create(ISettings settings);
        Task<string> CreateClientCredential(ISettings settings, ClientCredential clientCredential);
    }
}
