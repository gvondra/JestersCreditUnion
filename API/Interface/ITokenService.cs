using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ITokenService
    {
        Task<string> Create(ISettings settings);
    }
}
