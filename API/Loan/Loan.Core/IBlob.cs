using JestersCreditUnion.Loan.Framework;
using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public interface IBlob
    {
        Task Upload(ISettings settings, string name, Stream stream);
        Task<Stream> Download(ISettings settings, string name);
    }
}
