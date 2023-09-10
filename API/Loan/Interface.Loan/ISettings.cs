using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ISettings
    {
        string BaseAddress { get; }

        Task<string> GetToken();
    }
}
