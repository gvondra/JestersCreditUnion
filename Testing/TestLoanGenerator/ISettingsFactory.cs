using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface ISettingsFactory
    {
        Task<ApiSettings> GetApiSettings();
        Task<LoanApiSettings> GetLoanApiSettings();
    }
}
