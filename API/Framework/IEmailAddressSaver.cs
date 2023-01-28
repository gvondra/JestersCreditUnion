using JestersCreditUnion.CommonCore;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IEmailAddressSaver
    {
        Task Create(ISettings settings, params IEmailAddress[] emailAddresses);
    }
}
