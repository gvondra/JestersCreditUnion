using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IIdentificationCardReader
    {
        Task<Stream> ReadBorrowerIdentificationCard(ISettings settings);
    }
}
