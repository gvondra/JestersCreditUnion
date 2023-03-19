using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ITraceService
    {
        Task<List<string>> GetEventCodes(ISettings settings);
        Task<List<Trace>> Search(ISettings settings, DateTime maxTimestamp, string eventCode);
    }
}
