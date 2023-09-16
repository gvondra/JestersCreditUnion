using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface IWorkTaskConfigurationService
    {
        Task<WorkTaskConfiguration> Get(ISettings settings);
        Task<WorkTaskConfiguration> Save(ISettings settings, WorkTaskConfiguration workTaskConfiguration);
    }
}
