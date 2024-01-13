using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskTypeService
    {
        Task<List<WorkTaskType>> GetAll(ISettings settings);
        Task<WorkTaskType> GetByCode(ISettings settings, string code);
        Task<WorkTaskType> Get(ISettings settings, Guid id);
        Task<List<WorkTaskType>> GetByWorkGroupId(ISettings settings, Guid workGroupId);
        Task<WorkTaskType> Create(ISettings settings, WorkTaskType workTaskType);
        Task<WorkTaskType> Update(ISettings settings, WorkTaskType workTaskType);
        Task<string> LookupCode(ISettings settings, string name);
    }
}
