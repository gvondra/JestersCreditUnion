using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IExceptionService
    {
        Task<List<JestersCreditUnion.Interface.Models.Exception>> Search(ISettings settings, DateTime maxTimestamp);
    }
}
