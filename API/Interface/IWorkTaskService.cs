using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskService
    {
        Task<List<Models.WorkTask>> GetByWorkGroupId(ISettings settings, Guid workGroupId, bool? includeClosed = null);
        Task<Models.ClaimWorkTaskResponse> Claim(ISettings settings, Guid id, string assignToUserId, DateTime? assignedDate = null);
    }
}
