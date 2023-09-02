using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IAddressSaver
    {
        Task Create(ISettings settings, IAddress address);
    }
}
