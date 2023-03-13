using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskStatusService
    {
        Task<List<WorkTaskStatus>> GetAll(ISettings settings, Guid workTaskTypeId);
        Task<WorkTaskStatus> Get(ISettings settings, Guid workTaskTypeId, Guid id);
        Task<WorkTaskStatus> Create(ISettings settings, WorkTaskStatus workTaskStatus);
        Task<WorkTaskStatus> Update(ISettings settings, WorkTaskStatus workTaskStatus);
    }
}
