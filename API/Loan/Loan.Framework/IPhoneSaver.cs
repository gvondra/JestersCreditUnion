using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPhoneSaver
    {
        Task Create(ISettings settings, IPhone phone);
    }
}
