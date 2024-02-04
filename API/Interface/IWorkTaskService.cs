using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskService
    {
        Task<List<Models.WorkTask>> GetByWorkGroupId(ISettings settings, Guid workGroupId, bool? includeClosed = null);
        Task<List<Models.WorkTask>> GetByContext(ISettings settings, short referenceType, string referenceValue, bool? includeClosed = null);
        Task<Models.ClaimWorkTaskResponse> Claim(ISettings settings, Guid id, string assignToUserId, DateTime? assignedDate = null);
        Task<Models.WorkTask> Create(ISettings settings, Models.WorkTask workTask);
        Task<Models.WorkTask> Update(ISettings settings, Models.WorkTask workTask);
        Task<List<Models.WorkTask>> Patch(ISettings settings, IEnumerable<Dictionary<string, object>> patchData);
    }
}
