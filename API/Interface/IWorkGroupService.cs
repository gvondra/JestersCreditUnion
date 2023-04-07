using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkGroupService
    {
        Task<List<WorkGroup>> GetAll(ISettings settings);
        Task<WorkGroup> Get(ISettings settings, Guid id);
        Task<WorkGroup> Create(ISettings settings, WorkGroup workGroup);
        Task<WorkGroup> Update(ISettings settings, WorkGroup workGroup);
        Task AddWorkTaskTypeLink(ISettings settings, Guid workGroupId, Guid workTaskTypeId);
        Task DeleteWorkTaskTypeLink(ISettings settings, Guid workGroupId, Guid workTaskTypeId);
    }
}
