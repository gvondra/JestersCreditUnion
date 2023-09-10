using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IEmailAddressSaver
    {
        Task Create(ISettings settings, IEmailAddress emailAddress);
    }
}
