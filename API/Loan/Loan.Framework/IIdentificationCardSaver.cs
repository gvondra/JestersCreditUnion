using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IIdentificationCardSaver
    {
        Task SaveBorrowerIdentificationCard(ISettings settings, Stream stream);
    }
}
