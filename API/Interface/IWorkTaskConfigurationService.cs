using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskConfigurationService
    {
        Task<WorkTaskConfiguration> Get(ISettings settings);
        Task<WorkTaskConfiguration> Save(ISettings settings, WorkTaskConfiguration workTaskConfiguration);
    }
}
