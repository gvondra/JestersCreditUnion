using JestersCreditUnion.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public interface IBlob
    {
        Task Upload(ISettings settings, string name);
    }
}
