using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IIdentificationCardSaver
    {
        Task SaveBorrowerIdentificationCard(ISettings settings, Stream stream);
    }
}
