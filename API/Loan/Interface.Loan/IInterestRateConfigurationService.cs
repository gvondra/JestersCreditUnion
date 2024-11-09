using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface IInterestRateConfigurationService
    {
        Task<InterestRateConfiguration> Get(ISettings settings);
        Task<InterestRateConfiguration> Save(ISettings settings, InterestRateConfiguration interestRateConfiguration);
    }
}
