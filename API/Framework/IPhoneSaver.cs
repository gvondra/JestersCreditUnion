using JestersCreditUnion.CommonCore;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IPhoneSaver
    {
        Task Create(ISettings settings, params IPhone[] phones);
    }
}
