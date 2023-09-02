using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskConfigurationService
    {
        Task<WorkTaskConfiguration> Get(ISettings settings);
        Task<WorkTaskConfiguration> Save(ISettings settings, WorkTaskConfiguration workTaskConfiguration);
    }
}
