using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IWorkTaskService
    {
        Task<List<Models.WorkTask>> GetByWorkGroupId(ISettings settings, Guid workGroupId, bool? includeClosed = null);
    }
}
