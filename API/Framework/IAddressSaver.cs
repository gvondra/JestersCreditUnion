using JestersCreditUnion.CommonCore;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IAddressSaver
    {
        Task Create(ISettings settings, params IAddress[] addresses);
    }
}
