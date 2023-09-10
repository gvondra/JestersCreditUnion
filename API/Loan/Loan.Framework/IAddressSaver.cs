using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IAddressSaver
    {
        Task Create(ISettings settings, IAddress address);
    }
}
